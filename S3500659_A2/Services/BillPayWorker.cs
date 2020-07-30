using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using S3500659_A2.Data;
using S3500659_A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace S3500659_A2.Services
{
    public class BillPayWorker : BackgroundService
    {
        readonly ILogger<BillPayWorker> _logger;
        private readonly IServiceProvider _provider;

        public BillPayWorker(ILogger<BillPayWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _provider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                using IServiceScope scope = _provider.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                var billPays = await context.BillPays.ToListAsync();
                var customers = await context.Customers.ToListAsync();
                if (billPays.Any())
                {
                    foreach (var b in billPays)
                    {
                        if (b.Account.Balance >= b.Amount)
                        {
                            if (b.ScheduleDate <= DateTime.UtcNow)
                            {
                                if (b.Period == Period.Minute)
                                {
                                    b.Account.BillPay(b.Amount, "Bill Pay (Period: Minute)");
                                    b.ScheduleDate += TimeSpan.FromMinutes(1);
                                }
                                else if (b.Period == Period.Quarterly)
                                {
                                    b.Account.BillPay(b.Amount, "Bill Pay (Period: Quarterly)");
                                    b.ScheduleDate += TimeSpan.FromDays(91.25);
                                }
                                else if (b.Period == Period.Annually)
                                {
                                    b.Account.BillPay(b.Amount, "Bill Pay (Period: Yearly)");
                                    b.ScheduleDate += TimeSpan.FromDays(365);
                                }
                                else
                                {
                                    b.Account.BillPay(b.Amount, "Bill Pay (Period: Once off)");
                                    context.BillPays.Remove(b);
                                }
                                await context.SaveChangesAsync();
                            }
                        }

                    }

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }

        }

    }
}
