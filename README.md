# CronJobs
.net core 3.1 webapi project with Quartz and MongoDB ,which send request on your schedule.


#中文文档
使用反射自动生成Quartz的IJobDetail，然后使用默认JobExecutor为IJob的实现，请求对应网址。
使用观察者模式监听每次任务执行，并记录。
如有需要，可以设置每次执行时发送邮件提醒。