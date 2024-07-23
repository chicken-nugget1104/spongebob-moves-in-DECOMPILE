using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class GameCenterManager : AbstractManager
{
	// Token: 0x060005B7 RID: 1463 RVA: 0x00015E3C File Offset: 0x0001403C
	static GameCenterManager()
	{
		AbstractManager.initialize(typeof(GameCenterManager));
	}

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x060005B8 RID: 1464 RVA: 0x00015E50 File Offset: 0x00014050
	// (remove) Token: 0x060005B9 RID: 1465 RVA: 0x00015E68 File Offset: 0x00014068
	public static event Action<string> loadPlayerDataFailedEvent;

	// Token: 0x14000068 RID: 104
	// (add) Token: 0x060005BA RID: 1466 RVA: 0x00015E80 File Offset: 0x00014080
	// (remove) Token: 0x060005BB RID: 1467 RVA: 0x00015E98 File Offset: 0x00014098
	public static event Action<List<GameCenterPlayer>> playerDataLoadedEvent;

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x060005BC RID: 1468 RVA: 0x00015EB0 File Offset: 0x000140B0
	// (remove) Token: 0x060005BD RID: 1469 RVA: 0x00015EC8 File Offset: 0x000140C8
	public static event Action playerAuthenticatedEvent;

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x060005BE RID: 1470 RVA: 0x00015EE0 File Offset: 0x000140E0
	// (remove) Token: 0x060005BF RID: 1471 RVA: 0x00015EF8 File Offset: 0x000140F8
	public static event Action<string> playerFailedToAuthenticateEvent;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x060005C0 RID: 1472 RVA: 0x00015F10 File Offset: 0x00014110
	// (remove) Token: 0x060005C1 RID: 1473 RVA: 0x00015F28 File Offset: 0x00014128
	public static event Action playerLoggedOutEvent;

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x060005C2 RID: 1474 RVA: 0x00015F40 File Offset: 0x00014140
	// (remove) Token: 0x060005C3 RID: 1475 RVA: 0x00015F58 File Offset: 0x00014158
	public static event Action<string> profilePhotoLoadedEvent;

	// Token: 0x1400006D RID: 109
	// (add) Token: 0x060005C4 RID: 1476 RVA: 0x00015F70 File Offset: 0x00014170
	// (remove) Token: 0x060005C5 RID: 1477 RVA: 0x00015F88 File Offset: 0x00014188
	public static event Action<string> profilePhotoFailedEvent;

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x060005C6 RID: 1478 RVA: 0x00015FA0 File Offset: 0x000141A0
	// (remove) Token: 0x060005C7 RID: 1479 RVA: 0x00015FB8 File Offset: 0x000141B8
	public static event Action<Dictionary<string, string>> generateIdentityVerificationSignatureSucceededEvent;

	// Token: 0x1400006F RID: 111
	// (add) Token: 0x060005C8 RID: 1480 RVA: 0x00015FD0 File Offset: 0x000141D0
	// (remove) Token: 0x060005C9 RID: 1481 RVA: 0x00015FE8 File Offset: 0x000141E8
	public static event Action<string> generateIdentityVerificationSignatureFailedEvent;

	// Token: 0x14000070 RID: 112
	// (add) Token: 0x060005CA RID: 1482 RVA: 0x00016000 File Offset: 0x00014200
	// (remove) Token: 0x060005CB RID: 1483 RVA: 0x00016018 File Offset: 0x00014218
	public static event Action<string> loadCategoryTitlesFailedEvent;

	// Token: 0x14000071 RID: 113
	// (add) Token: 0x060005CC RID: 1484 RVA: 0x00016030 File Offset: 0x00014230
	// (remove) Token: 0x060005CD RID: 1485 RVA: 0x00016048 File Offset: 0x00014248
	public static event Action<List<GameCenterLeaderboard>> categoriesLoadedEvent;

	// Token: 0x14000072 RID: 114
	// (add) Token: 0x060005CE RID: 1486 RVA: 0x00016060 File Offset: 0x00014260
	// (remove) Token: 0x060005CF RID: 1487 RVA: 0x00016078 File Offset: 0x00014278
	public static event Action<string> reportScoreFailedEvent;

	// Token: 0x14000073 RID: 115
	// (add) Token: 0x060005D0 RID: 1488 RVA: 0x00016090 File Offset: 0x00014290
	// (remove) Token: 0x060005D1 RID: 1489 RVA: 0x000160A8 File Offset: 0x000142A8
	public static event Action<string> reportScoreFinishedEvent;

	// Token: 0x14000074 RID: 116
	// (add) Token: 0x060005D2 RID: 1490 RVA: 0x000160C0 File Offset: 0x000142C0
	// (remove) Token: 0x060005D3 RID: 1491 RVA: 0x000160D8 File Offset: 0x000142D8
	public static event Action<string> retrieveScoresFailedEvent;

	// Token: 0x14000075 RID: 117
	// (add) Token: 0x060005D4 RID: 1492 RVA: 0x000160F0 File Offset: 0x000142F0
	// (remove) Token: 0x060005D5 RID: 1493 RVA: 0x00016108 File Offset: 0x00014308
	public static event Action<GameCenterRetrieveScoresResult> scoresLoadedEvent;

	// Token: 0x14000076 RID: 118
	// (add) Token: 0x060005D6 RID: 1494 RVA: 0x00016120 File Offset: 0x00014320
	// (remove) Token: 0x060005D7 RID: 1495 RVA: 0x00016138 File Offset: 0x00014338
	public static event Action<string> retrieveScoresForPlayerIdFailedEvent;

	// Token: 0x14000077 RID: 119
	// (add) Token: 0x060005D8 RID: 1496 RVA: 0x00016150 File Offset: 0x00014350
	// (remove) Token: 0x060005D9 RID: 1497 RVA: 0x00016168 File Offset: 0x00014368
	public static event Action<GameCenterRetrieveScoresResult> scoresForPlayerIdLoadedEvent;

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x060005DA RID: 1498 RVA: 0x00016180 File Offset: 0x00014380
	// (remove) Token: 0x060005DB RID: 1499 RVA: 0x00016198 File Offset: 0x00014398
	public static event Action<string> reportAchievementFailedEvent;

	// Token: 0x14000079 RID: 121
	// (add) Token: 0x060005DC RID: 1500 RVA: 0x000161B0 File Offset: 0x000143B0
	// (remove) Token: 0x060005DD RID: 1501 RVA: 0x000161C8 File Offset: 0x000143C8
	public static event Action<string> reportAchievementFinishedEvent;

	// Token: 0x1400007A RID: 122
	// (add) Token: 0x060005DE RID: 1502 RVA: 0x000161E0 File Offset: 0x000143E0
	// (remove) Token: 0x060005DF RID: 1503 RVA: 0x000161F8 File Offset: 0x000143F8
	public static event Action<string> loadAchievementsFailedEvent;

	// Token: 0x1400007B RID: 123
	// (add) Token: 0x060005E0 RID: 1504 RVA: 0x00016210 File Offset: 0x00014410
	// (remove) Token: 0x060005E1 RID: 1505 RVA: 0x00016228 File Offset: 0x00014428
	public static event Action<List<GameCenterAchievement>> achievementsLoadedEvent;

	// Token: 0x1400007C RID: 124
	// (add) Token: 0x060005E2 RID: 1506 RVA: 0x00016240 File Offset: 0x00014440
	// (remove) Token: 0x060005E3 RID: 1507 RVA: 0x00016258 File Offset: 0x00014458
	public static event Action<string> resetAchievementsFailedEvent;

	// Token: 0x1400007D RID: 125
	// (add) Token: 0x060005E4 RID: 1508 RVA: 0x00016270 File Offset: 0x00014470
	// (remove) Token: 0x060005E5 RID: 1509 RVA: 0x00016288 File Offset: 0x00014488
	public static event Action resetAchievementsFinishedEvent;

	// Token: 0x1400007E RID: 126
	// (add) Token: 0x060005E6 RID: 1510 RVA: 0x000162A0 File Offset: 0x000144A0
	// (remove) Token: 0x060005E7 RID: 1511 RVA: 0x000162B8 File Offset: 0x000144B8
	public static event Action<string> retrieveAchievementMetadataFailedEvent;

	// Token: 0x1400007F RID: 127
	// (add) Token: 0x060005E8 RID: 1512 RVA: 0x000162D0 File Offset: 0x000144D0
	// (remove) Token: 0x060005E9 RID: 1513 RVA: 0x000162E8 File Offset: 0x000144E8
	public static event Action<List<GameCenterAchievementMetadata>> achievementMetadataLoadedEvent;

	// Token: 0x14000080 RID: 128
	// (add) Token: 0x060005EA RID: 1514 RVA: 0x00016300 File Offset: 0x00014500
	// (remove) Token: 0x060005EB RID: 1515 RVA: 0x00016318 File Offset: 0x00014518
	public static event Action<string> selectChallengeablePlayerIDsDidFailEvent;

	// Token: 0x14000081 RID: 129
	// (add) Token: 0x060005EC RID: 1516 RVA: 0x00016330 File Offset: 0x00014530
	// (remove) Token: 0x060005ED RID: 1517 RVA: 0x00016348 File Offset: 0x00014548
	public static event Action<List<object>> selectChallengeablePlayerIDsDidFinishEvent;

	// Token: 0x14000082 RID: 130
	// (add) Token: 0x060005EE RID: 1518 RVA: 0x00016360 File Offset: 0x00014560
	// (remove) Token: 0x060005EF RID: 1519 RVA: 0x00016378 File Offset: 0x00014578
	public static event Action<GameCenterChallenge> localPlayerDidSelectChallengeEvent;

	// Token: 0x14000083 RID: 131
	// (add) Token: 0x060005F0 RID: 1520 RVA: 0x00016390 File Offset: 0x00014590
	// (remove) Token: 0x060005F1 RID: 1521 RVA: 0x000163A8 File Offset: 0x000145A8
	public static event Action<GameCenterChallenge> localPlayerDidCompleteChallengeEvent;

	// Token: 0x14000084 RID: 132
	// (add) Token: 0x060005F2 RID: 1522 RVA: 0x000163C0 File Offset: 0x000145C0
	// (remove) Token: 0x060005F3 RID: 1523 RVA: 0x000163D8 File Offset: 0x000145D8
	public static event Action<GameCenterChallenge> remotePlayerDidCompleteChallengeEvent;

	// Token: 0x14000085 RID: 133
	// (add) Token: 0x060005F4 RID: 1524 RVA: 0x000163F0 File Offset: 0x000145F0
	// (remove) Token: 0x060005F5 RID: 1525 RVA: 0x00016408 File Offset: 0x00014608
	public static event Action<List<GameCenterChallenge>> challengesLoadedEvent;

	// Token: 0x14000086 RID: 134
	// (add) Token: 0x060005F6 RID: 1526 RVA: 0x00016420 File Offset: 0x00014620
	// (remove) Token: 0x060005F7 RID: 1527 RVA: 0x00016438 File Offset: 0x00014638
	public static event Action<string> challengesFailedToLoadEvent;

	// Token: 0x14000087 RID: 135
	// (add) Token: 0x060005F8 RID: 1528 RVA: 0x00016450 File Offset: 0x00014650
	// (remove) Token: 0x060005F9 RID: 1529 RVA: 0x00016468 File Offset: 0x00014668
	public static event Action<List<object>> challengeIssuedSuccessfullyEvent;

	// Token: 0x14000088 RID: 136
	// (add) Token: 0x060005FA RID: 1530 RVA: 0x00016480 File Offset: 0x00014680
	// (remove) Token: 0x060005FB RID: 1531 RVA: 0x00016498 File Offset: 0x00014698
	public static event Action challengeNotIssuedEvent;

	// Token: 0x14000089 RID: 137
	// (add) Token: 0x060005FC RID: 1532 RVA: 0x000164B0 File Offset: 0x000146B0
	// (remove) Token: 0x060005FD RID: 1533 RVA: 0x000164C8 File Offset: 0x000146C8
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> loadPlayerDataFailed;

	// Token: 0x1400008A RID: 138
	// (add) Token: 0x060005FE RID: 1534 RVA: 0x000164E0 File Offset: 0x000146E0
	// (remove) Token: 0x060005FF RID: 1535 RVA: 0x000164F8 File Offset: 0x000146F8
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<List<GameCenterPlayer>> playerDataLoaded;

	// Token: 0x1400008B RID: 139
	// (add) Token: 0x06000600 RID: 1536 RVA: 0x00016510 File Offset: 0x00014710
	// (remove) Token: 0x06000601 RID: 1537 RVA: 0x00016528 File Offset: 0x00014728
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action playerAuthenticated;

	// Token: 0x1400008C RID: 140
	// (add) Token: 0x06000602 RID: 1538 RVA: 0x00016540 File Offset: 0x00014740
	// (remove) Token: 0x06000603 RID: 1539 RVA: 0x00016558 File Offset: 0x00014758
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> playerFailedToAuthenticate;

	// Token: 0x1400008D RID: 141
	// (add) Token: 0x06000604 RID: 1540 RVA: 0x00016570 File Offset: 0x00014770
	// (remove) Token: 0x06000605 RID: 1541 RVA: 0x00016588 File Offset: 0x00014788
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action playerLoggedOut;

	// Token: 0x1400008E RID: 142
	// (add) Token: 0x06000606 RID: 1542 RVA: 0x000165A0 File Offset: 0x000147A0
	// (remove) Token: 0x06000607 RID: 1543 RVA: 0x000165B8 File Offset: 0x000147B8
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> profilePhotoLoaded;

	// Token: 0x1400008F RID: 143
	// (add) Token: 0x06000608 RID: 1544 RVA: 0x000165D0 File Offset: 0x000147D0
	// (remove) Token: 0x06000609 RID: 1545 RVA: 0x000165E8 File Offset: 0x000147E8
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> profilePhotoFailed;

	// Token: 0x14000090 RID: 144
	// (add) Token: 0x0600060A RID: 1546 RVA: 0x00016600 File Offset: 0x00014800
	// (remove) Token: 0x0600060B RID: 1547 RVA: 0x00016618 File Offset: 0x00014818
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> loadCategoryTitlesFailed;

	// Token: 0x14000091 RID: 145
	// (add) Token: 0x0600060C RID: 1548 RVA: 0x00016630 File Offset: 0x00014830
	// (remove) Token: 0x0600060D RID: 1549 RVA: 0x00016648 File Offset: 0x00014848
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<List<GameCenterLeaderboard>> categoriesLoaded;

	// Token: 0x14000092 RID: 146
	// (add) Token: 0x0600060E RID: 1550 RVA: 0x00016660 File Offset: 0x00014860
	// (remove) Token: 0x0600060F RID: 1551 RVA: 0x00016678 File Offset: 0x00014878
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> reportScoreFailed;

	// Token: 0x14000093 RID: 147
	// (add) Token: 0x06000610 RID: 1552 RVA: 0x00016690 File Offset: 0x00014890
	// (remove) Token: 0x06000611 RID: 1553 RVA: 0x000166A8 File Offset: 0x000148A8
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> reportScoreFinished;

	// Token: 0x14000094 RID: 148
	// (add) Token: 0x06000612 RID: 1554 RVA: 0x000166C0 File Offset: 0x000148C0
	// (remove) Token: 0x06000613 RID: 1555 RVA: 0x000166D8 File Offset: 0x000148D8
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> retrieveScoresFailed;

	// Token: 0x14000095 RID: 149
	// (add) Token: 0x06000614 RID: 1556 RVA: 0x000166F0 File Offset: 0x000148F0
	// (remove) Token: 0x06000615 RID: 1557 RVA: 0x00016708 File Offset: 0x00014908
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<List<GameCenterScore>> scoresLoaded;

	// Token: 0x14000096 RID: 150
	// (add) Token: 0x06000616 RID: 1558 RVA: 0x00016720 File Offset: 0x00014920
	// (remove) Token: 0x06000617 RID: 1559 RVA: 0x00016738 File Offset: 0x00014938
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> retrieveScoresForPlayerIdFailed;

	// Token: 0x14000097 RID: 151
	// (add) Token: 0x06000618 RID: 1560 RVA: 0x00016750 File Offset: 0x00014950
	// (remove) Token: 0x06000619 RID: 1561 RVA: 0x00016768 File Offset: 0x00014968
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<List<GameCenterScore>> scoresForPlayerIdLoaded;

	// Token: 0x14000098 RID: 152
	// (add) Token: 0x0600061A RID: 1562 RVA: 0x00016780 File Offset: 0x00014980
	// (remove) Token: 0x0600061B RID: 1563 RVA: 0x00016798 File Offset: 0x00014998
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> reportAchievementFailed;

	// Token: 0x14000099 RID: 153
	// (add) Token: 0x0600061C RID: 1564 RVA: 0x000167B0 File Offset: 0x000149B0
	// (remove) Token: 0x0600061D RID: 1565 RVA: 0x000167C8 File Offset: 0x000149C8
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> reportAchievementFinished;

	// Token: 0x1400009A RID: 154
	// (add) Token: 0x0600061E RID: 1566 RVA: 0x000167E0 File Offset: 0x000149E0
	// (remove) Token: 0x0600061F RID: 1567 RVA: 0x000167F8 File Offset: 0x000149F8
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> loadAchievementsFailed;

	// Token: 0x1400009B RID: 155
	// (add) Token: 0x06000620 RID: 1568 RVA: 0x00016810 File Offset: 0x00014A10
	// (remove) Token: 0x06000621 RID: 1569 RVA: 0x00016828 File Offset: 0x00014A28
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<List<GameCenterAchievement>> achievementsLoaded;

	// Token: 0x1400009C RID: 156
	// (add) Token: 0x06000622 RID: 1570 RVA: 0x00016840 File Offset: 0x00014A40
	// (remove) Token: 0x06000623 RID: 1571 RVA: 0x00016858 File Offset: 0x00014A58
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> resetAchievementsFailed;

	// Token: 0x1400009D RID: 157
	// (add) Token: 0x06000624 RID: 1572 RVA: 0x00016870 File Offset: 0x00014A70
	// (remove) Token: 0x06000625 RID: 1573 RVA: 0x00016888 File Offset: 0x00014A88
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action resetAchievementsFinished;

	// Token: 0x1400009E RID: 158
	// (add) Token: 0x06000626 RID: 1574 RVA: 0x000168A0 File Offset: 0x00014AA0
	// (remove) Token: 0x06000627 RID: 1575 RVA: 0x000168B8 File Offset: 0x00014AB8
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<string> retrieveAchievementMetadataFailed;

	// Token: 0x1400009F RID: 159
	// (add) Token: 0x06000628 RID: 1576 RVA: 0x000168D0 File Offset: 0x00014AD0
	// (remove) Token: 0x06000629 RID: 1577 RVA: 0x000168E8 File Offset: 0x00014AE8
	[Obsolete("All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name.")]
	public static event Action<List<GameCenterAchievementMetadata>> achievementMetadataLoaded;

	// Token: 0x0600062A RID: 1578 RVA: 0x00016900 File Offset: 0x00014B00
	public void loadPlayerDataDidFail(string error)
	{
		if (GameCenterManager.loadPlayerDataFailedEvent != null)
		{
			GameCenterManager.loadPlayerDataFailedEvent(error);
		}
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x00016918 File Offset: 0x00014B18
	public void loadPlayerDataDidLoad(string jsonFriendList)
	{
		List<GameCenterPlayer> obj = GameCenterPlayer.fromJSON(jsonFriendList);
		if (GameCenterManager.playerDataLoadedEvent != null)
		{
			GameCenterManager.playerDataLoadedEvent(obj);
		}
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x00016944 File Offset: 0x00014B44
	public void playerDidLogOut()
	{
		if (GameCenterManager.playerLoggedOutEvent != null)
		{
			GameCenterManager.playerLoggedOutEvent();
		}
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0001695C File Offset: 0x00014B5C
	public void playerDidAuthenticate(string playerId)
	{
		if (GameCenterManager.playerAuthenticatedEvent != null)
		{
			GameCenterManager.playerAuthenticatedEvent();
		}
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x00016974 File Offset: 0x00014B74
	public void playerAuthenticationFailed(string error)
	{
		if (GameCenterManager.playerFailedToAuthenticateEvent != null)
		{
			GameCenterManager.playerFailedToAuthenticateEvent(error);
		}
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0001698C File Offset: 0x00014B8C
	public void loadProfilePhotoDidLoad(string path)
	{
		if (GameCenterManager.profilePhotoLoadedEvent != null)
		{
			GameCenterManager.profilePhotoLoadedEvent(path);
		}
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x000169A4 File Offset: 0x00014BA4
	public void loadProfilePhotoDidFail(string error)
	{
		if (GameCenterManager.profilePhotoFailedEvent != null)
		{
			GameCenterManager.profilePhotoFailedEvent(error);
		}
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x000169BC File Offset: 0x00014BBC
	public void generateIdentityVerificationSignatureSucceeded(string json)
	{
		if (GameCenterManager.generateIdentityVerificationSignatureSucceededEvent != null)
		{
			GameCenterManager.generateIdentityVerificationSignatureSucceededEvent(Json.decode<Dictionary<string, string>>(json, null));
		}
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x000169DC File Offset: 0x00014BDC
	public void generateIdentityVerificationSignatureFailed(string error)
	{
		if (GameCenterManager.generateIdentityVerificationSignatureFailedEvent != null)
		{
			GameCenterManager.generateIdentityVerificationSignatureFailedEvent(error);
		}
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x000169F4 File Offset: 0x00014BF4
	public void loadCategoryTitlesDidFail(string error)
	{
		if (GameCenterManager.loadCategoryTitlesFailedEvent != null)
		{
			GameCenterManager.loadCategoryTitlesFailedEvent(error);
		}
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x00016A0C File Offset: 0x00014C0C
	public void categoriesDidLoad(string jsonCategoryList)
	{
		List<GameCenterLeaderboard> obj = GameCenterLeaderboard.fromJSON(jsonCategoryList);
		if (GameCenterManager.categoriesLoadedEvent != null)
		{
			GameCenterManager.categoriesLoadedEvent(obj);
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x00016A38 File Offset: 0x00014C38
	public void reportScoreDidFail(string error)
	{
		if (GameCenterManager.reportScoreFailedEvent != null)
		{
			GameCenterManager.reportScoreFailedEvent(error);
		}
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x00016A50 File Offset: 0x00014C50
	public void reportScoreDidFinish(string category)
	{
		if (GameCenterManager.reportScoreFinishedEvent != null)
		{
			GameCenterManager.reportScoreFinishedEvent(category);
		}
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x00016A68 File Offset: 0x00014C68
	public void retrieveScoresDidFail(string category)
	{
		if (GameCenterManager.retrieveScoresFailedEvent != null)
		{
			GameCenterManager.retrieveScoresFailedEvent(category);
		}
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x00016A80 File Offset: 0x00014C80
	public void retrieveScoresDidLoad(string json)
	{
		Debug.Log(json);
		if (GameCenterManager.scoresLoadedEvent != null)
		{
			GameCenterManager.scoresLoadedEvent(Json.decode<GameCenterRetrieveScoresResult>(json, null));
		}
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x00016AA4 File Offset: 0x00014CA4
	public void retrieveScoresForPlayerIdDidFail(string error)
	{
		if (GameCenterManager.retrieveScoresForPlayerIdFailedEvent != null)
		{
			GameCenterManager.retrieveScoresForPlayerIdFailedEvent(error);
		}
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x00016ABC File Offset: 0x00014CBC
	public void retrieveScoresForPlayerIdDidLoad(string json)
	{
		if (GameCenterManager.scoresForPlayerIdLoadedEvent != null)
		{
			GameCenterManager.scoresForPlayerIdLoadedEvent(Json.decode<GameCenterRetrieveScoresResult>(json, null));
		}
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00016ADC File Offset: 0x00014CDC
	public void reportAchievementDidFail(string error)
	{
		if (GameCenterManager.reportAchievementFailedEvent != null)
		{
			GameCenterManager.reportAchievementFailedEvent(error);
		}
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00016AF4 File Offset: 0x00014CF4
	public void reportAchievementDidFinish(string identifier)
	{
		if (GameCenterManager.reportAchievementFinishedEvent != null)
		{
			GameCenterManager.reportAchievementFinishedEvent(identifier);
		}
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00016B0C File Offset: 0x00014D0C
	public void loadAchievementsDidFail(string error)
	{
		if (GameCenterManager.loadAchievementsFailedEvent != null)
		{
			GameCenterManager.loadAchievementsFailedEvent(error);
		}
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00016B24 File Offset: 0x00014D24
	public void achievementsDidLoad(string jsonAchievmentList)
	{
		List<GameCenterAchievement> obj = GameCenterAchievement.fromJSON(jsonAchievmentList);
		if (GameCenterManager.achievementsLoadedEvent != null)
		{
			GameCenterManager.achievementsLoadedEvent(obj);
		}
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00016B50 File Offset: 0x00014D50
	public void resetAchievementsDidFail(string error)
	{
		if (GameCenterManager.resetAchievementsFailedEvent != null)
		{
			GameCenterManager.resetAchievementsFailedEvent(error);
		}
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x00016B68 File Offset: 0x00014D68
	public void resetAchievementsDidFinish(string emptyString)
	{
		if (GameCenterManager.resetAchievementsFinishedEvent != null)
		{
			GameCenterManager.resetAchievementsFinishedEvent();
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00016B80 File Offset: 0x00014D80
	public void retrieveAchievementsMetaDataDidFail(string error)
	{
		if (GameCenterManager.retrieveAchievementMetadataFailedEvent != null)
		{
			GameCenterManager.retrieveAchievementMetadataFailedEvent(error);
		}
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x00016B98 File Offset: 0x00014D98
	public void achievementMetadataDidLoad(string jsonAchievementDescriptionList)
	{
		List<GameCenterAchievementMetadata> obj = GameCenterAchievementMetadata.fromJSON(jsonAchievementDescriptionList);
		if (GameCenterManager.achievementMetadataLoadedEvent != null)
		{
			GameCenterManager.achievementMetadataLoadedEvent(obj);
		}
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00016BC4 File Offset: 0x00014DC4
	public void selectChallengeablePlayerIDsDidFail(string error)
	{
		if (GameCenterManager.selectChallengeablePlayerIDsDidFailEvent != null)
		{
			GameCenterManager.selectChallengeablePlayerIDsDidFailEvent(error);
		}
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x00016BDC File Offset: 0x00014DDC
	public void selectChallengeablePlayerIDsDidFinish(string json)
	{
		if (GameCenterManager.selectChallengeablePlayerIDsDidFinishEvent != null)
		{
			GameCenterManager.selectChallengeablePlayerIDsDidFinishEvent(json.listFromJson());
		}
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x00016BF8 File Offset: 0x00014DF8
	public void localPlayerDidSelectChallenge(string json)
	{
		if (GameCenterManager.localPlayerDidSelectChallengeEvent != null)
		{
			GameCenterManager.localPlayerDidSelectChallengeEvent(new GameCenterChallenge(json.dictionaryFromJson()));
		}
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x00016C1C File Offset: 0x00014E1C
	public void localPlayerDidCompleteChallenge(string json)
	{
		if (GameCenterManager.localPlayerDidCompleteChallengeEvent != null)
		{
			GameCenterManager.localPlayerDidCompleteChallengeEvent(new GameCenterChallenge(json.dictionaryFromJson()));
		}
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x00016C40 File Offset: 0x00014E40
	public void remotePlayerDidCompleteChallenge(string json)
	{
		if (GameCenterManager.remotePlayerDidCompleteChallengeEvent != null)
		{
			GameCenterManager.remotePlayerDidCompleteChallengeEvent(new GameCenterChallenge(json.dictionaryFromJson()));
		}
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00016C64 File Offset: 0x00014E64
	public void challengesLoaded(string json)
	{
		if (GameCenterManager.challengesLoadedEvent != null)
		{
			GameCenterManager.challengesLoadedEvent(GameCenterChallenge.fromJson(json));
		}
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x00016C80 File Offset: 0x00014E80
	public void challengesFailedToLoad(string error)
	{
		if (GameCenterManager.challengesFailedToLoadEvent != null)
		{
			GameCenterManager.challengesFailedToLoadEvent(error);
		}
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x00016C98 File Offset: 0x00014E98
	public void challengeIssuedSuccessfully(string json)
	{
		if (GameCenterManager.challengeIssuedSuccessfullyEvent != null)
		{
			GameCenterManager.challengeIssuedSuccessfullyEvent(json.listFromJson());
		}
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00016CB4 File Offset: 0x00014EB4
	public void challengeNotIssued(string empty)
	{
		if (GameCenterManager.challengeNotIssuedEvent != null)
		{
			GameCenterManager.challengeNotIssuedEvent();
		}
	}
}
