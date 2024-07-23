using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200041D RID: 1053
public interface IDisplayController
{
	// Token: 0x06002079 RID: 8313
	IDisplayController Clone(DisplayControllerManager dcm);

	// Token: 0x0600207A RID: 8314
	IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false);

	// Token: 0x0600207B RID: 8315
	IDisplayController CloneAndSetVisible(DisplayControllerManager dcm);

	// Token: 0x0600207C RID: 8316
	void AddDisplayState(Dictionary<string, object> dict);

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x0600207D RID: 8317
	// (set) Token: 0x0600207E RID: 8318
	bool Visible { get; set; }

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x0600207F RID: 8319
	// (set) Token: 0x06002080 RID: 8320
	float Alpha { get; set; }

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06002081 RID: 8321
	// (set) Token: 0x06002082 RID: 8322
	Color Color { get; set; }

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06002083 RID: 8323
	bool IsDestroyed { get; }

	// Token: 0x06002084 RID: 8324
	bool Intersects(Ray ray);

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06002085 RID: 8325
	QuadHitObject HitObject { get; }

	// Token: 0x06002086 RID: 8326
	void DisplayState(string state);

	// Token: 0x06002087 RID: 8327
	void ChangeMesh(string state, string hitMeshName);

	// Token: 0x06002088 RID: 8328
	void UpdateMaterialOrTexture(string material);

	// Token: 0x06002089 RID: 8329
	void SetMaskPercentage(float pct);

	// Token: 0x0600208A RID: 8330
	string GetDisplayState();

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x0600208B RID: 8331
	string MaterialName { get; }

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x0600208C RID: 8332
	// (set) Token: 0x0600208D RID: 8333
	Vector3 Position { get; set; }

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x0600208E RID: 8334
	Transform Transform { get; }

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x0600208F RID: 8335
	// (set) Token: 0x06002090 RID: 8336
	Vector3 BillboardScaling { get; set; }

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06002091 RID: 8337
	// (set) Token: 0x06002092 RID: 8338
	Vector3 Scale { get; set; }

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06002093 RID: 8339
	Vector3 Forward { get; }

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06002094 RID: 8340
	Vector3 Up { get; }

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06002095 RID: 8341
	float Width { get; }

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06002096 RID: 8342
	float Height { get; }

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06002097 RID: 8343
	// (set) Token: 0x06002098 RID: 8344
	int LevelOfDetail { get; set; }

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06002099 RID: 8345
	int NumberOfLevelsOfDetail { get; }

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x0600209A RID: 8346
	int MaxLevelOfDetail { get; }

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x0600209B RID: 8347
	bool isVisible { get; }

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x0600209C RID: 8348
	// (set) Token: 0x0600209D RID: 8349
	string DefaultDisplayState { get; set; }

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x0600209E RID: 8350
	// (set) Token: 0x0600209F RID: 8351
	bool isPerspectiveInArt { get; set; }

	// Token: 0x060020A0 RID: 8352
	void Billboard(BillboardDelegate billboard);

	// Token: 0x060020A1 RID: 8353
	void OnUpdate(Camera sceneCamera, ParticleSystemManager psm);

	// Token: 0x060020A2 RID: 8354
	void Destroy();

	// Token: 0x060020A3 RID: 8355
	void AttachGUIElementToTarget(SBGUIElement element, string target);

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x060020A4 RID: 8356
	// (set) Token: 0x060020A5 RID: 8357
	DisplayControllerFlags Flags { get; set; }
}
