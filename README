# 目前Alt&Ctrl這類功能鍵有BUG
# MapleStoryTools
基於arduino leonardo r3 (atmega32u4)開發，其他型號不知道能不能用(328P晶片系列肯定不行)
腳本說明:
press (key) (ms)
wait (ms)
combo (key) (key) (ms)
(ms)為可選屬性(Optional)，若不設定則預設值是10~20
ms可以是範圍值 ex:wait 1000-2000, press a 20-100
機率觸發框架:
may (百分比)
mayend

------------Script example---------------
press 3 20
wait 1000-2000
combo left o 50
wait 500-1000
press a
wait 500-1000
may 25
press e
wait 500-1000
mayend
combo right o
wait 500-1000
press a
------------Script example---------------
盡量多穿插機率觸發框架讓你仿人類行為
