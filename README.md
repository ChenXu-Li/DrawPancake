unity2d 小游戏

玩法：
需要在有限步数内伸缩方块铺满平面

版本 2021.3.32

关卡：
在SampleScence场景中，通过createWorld脚本中的levelname切换关卡数据生成地图

关卡编辑器：
在editor场景中，通过EditorLevel组件中的EditType切换编辑模式，先使用PaintModel模式涂色确定范围，
再使用LocateModel模式确定砖块的左上和右下节点，通过BlocksInEditor添加删减元素。






