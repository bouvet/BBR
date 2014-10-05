﻿using System.Collections.Generic;
using BouvetCodeCamp.SpillSimulator.Jobs;
using Quartz;

namespace BouvetCodeCamp.SpillSimulator.Schedulers
{
    public class BaseScheduler
    {
        private readonly IScheduler _scheduler;

        public BaseScheduler(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void ScheduleHentGjeldendePost()
        {
            IJobDetail job = JobBuilder.Create<BaseGjeldendePostJob>()
                .WithIdentity("hentGjeldendePost", "baseGruppe")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("firstBase", "baseGruppe")
                .StartAt(DateBuilder.FutureDate(10, IntervalUnit.Second))
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .WithRepeatCount(5))
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }
    }
}