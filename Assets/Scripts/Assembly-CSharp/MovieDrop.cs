using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class MovieDrop : ItemDrop
{
	// Token: 0x06000D1A RID: 3354 RVA: 0x00050CE8 File Offset: 0x0004EEE8
	public MovieDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Action callback) : base(position, fixedOffset, direction, definition, creationTime, callback)
	{
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x00050CFC File Offset: 0x0004EEFC
	protected override void OnCollectionAnimationComplete(Session session)
	{
		session.TheSoundEffectManager.PlaySound("MovieCollected");
		session.TheGame.movieManager.UnlockMovie(this.definition.Did);
		MovieInfo movieInfoById = session.TheGame.movieManager.GetMovieInfoById(this.definition.Did);
		FoundMovieDialogInputData item = new FoundMovieDialogInputData(Language.Get("!!MOVIE_UNLOCKED_TITLE"), string.Format(Language.Get("!!MOVIE_UNLOCKED_DIALOG"), Language.Get(movieInfoById.CollectName)), "MovieIcon.png", movieInfoById.MovieFile, "Beat_FoundMovie");
		session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
		{
			item
		}, uint.MaxValue);
		this.onCleanupComplete();
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x00050DBC File Offset: 0x0004EFBC
	protected override bool UpdateCleanup(Session session, Camera camera, bool updateCollectionTimer)
	{
		return base.ExplodeInPlace(session, camera, updateCollectionTimer, "Prefabs/FX/Fx_Film_Roll", "MovieDropExplodeInPlace");
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x00050DD4 File Offset: 0x0004EFD4
	protected override void PlayTapAnimation(Session session)
	{
		session.TheSoundEffectManager.PlaySound("TapFallenMovieItem");
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x00050DE8 File Offset: 0x0004EFE8
	protected override void PlayRewardAmountTextAnim(Session session)
	{
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x00050DEC File Offset: 0x0004EFEC
	public static Vector2 GetScreenCollectionDestination()
	{
		return new Vector2((float)Screen.width * 0.854f, (float)Screen.height * 0.073f);
	}
}
