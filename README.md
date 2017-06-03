# Leox.Transaction
use Leox.Aop to implement transaction

基于我的项目Leox.Aop + ado.net 实现的一个抽象事务，类似spring中的 @Transactional.
在Aop的OnStart方法中启动一个事务，同时用一个ConcurrentDictionary来保存当前数据库连接
key为连接id（guid），value为Connection。当执行成功或出现异常时就在OnSuccess或OnException方法
中执行提交或回滚，连接Id始终保存在ThreadLocal中方便后端获取到SqlCommand操作相关方法。
