using Quartz;

IHost host = CreateHost(args);
await host.RunAsync();

IHost CreateHost(string[] args)
    => Host.CreateDefaultBuilder(args)
           .ConfigureServices(services =>
           {
               services.AddQuartz(q =>
               {
                   // 這邊寫上排程的相關設定 或 排程建立

                   // 可以使服務注入用 scoped 方式注入
                   q.UseMicrosoftDependencyInjectionJobFactory();

                   // 這邊都是預設值
                   q.UseSimpleTypeLoader();
                   q.UseInMemoryStore();
                   q.UseDefaultThreadPool(tp =>
                   {
                       // 併發數量
                       tp.MaxConcurrency = 10;
                   });

                   // 一秒後執行之後每秒執行
                   q.ScheduleJob<TimeJob>(trigger => trigger
                       .WithIdentity("TimeJob")
                       .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(1)))
                       .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
                   );

               });

               // Host 服務建立
               services.AddQuartzHostedService(options =>
               {
                   // 主程式關閉時，會確保當前任務已經完成
                   options.WaitForJobsToComplete = true;
               });

           })
           .Build();