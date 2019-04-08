#Attribute:特性类别
1.TrackGroup:针对TrackGroup
        label:控件名称
        TrackGenres:指定Group中可添加Track类型
2.TimelineTrack:针对TimelineTrack
        label:控件名称
        TrackGenres:指定Track所属Group类型
        TrackGenre:含义同上
        AllowedItemGenres:指定Track中可添加Item类型
3.CutsceneItem:
        category:分类名称
        label:控件名称
        genres:指定Item所属Track类型

#TimelineItem:自定义时间线项,必须派生于以下类型之一
CinemaGlobalAction  - 跨越一段时间（动作）的项目,并影响全局场景.
CinemaGlobalEvent  - 在特定时间点（事件）触发的项目,会影响全局场景.
CinemaActorAction  - 跨越一段时间（动作）的项目,并影响场景中的1个或多个actor.
CinemaActorEvent  - 在特定时间点（事件）触发的项目,并影响场景中的1个或多个actor.
#如果需要操纵一些cutscene中一些数据,你将有必要实现IRevertable接口,这将有助于进出播放模式时,场景能保持初始状态.
[注:播放模式下修改数据,在退出播放模式时,如果不保存导出数据,则全部丢失]


