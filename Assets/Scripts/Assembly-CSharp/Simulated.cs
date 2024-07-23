using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002AD RID: 685
public class Simulated
{
	// Token: 0x060014EE RID: 5358 RVA: 0x0008CDD8 File Offset: 0x0008AFD8
	public Simulated(Simulation simulation, Entity entity, Vector2 position)
	{
		this.entity = entity;
		this.footprint = (this.entity.Invariable["footprint"] as AlignedBox);
		this.Position = position;
		this.Position = position;
		if (this.entity.Invariable["dropshadow"] != null)
		{
			this.dropShadowDisplayController = new DeferredDisplayController(this.entity.Invariable["dropshadow"] as IDisplayController, simulation.EntityManager.DisplayControllerManager);
			this.dropShadowDisplayController.Visible = true;
		}
		this.machine = (this.entity.Invariable["machine"] as StateMachine<Simulated.StateAction, Command.TYPE>);
		foreach (Simulated.StateAction key in this.machine.States)
		{
			this.delegatedCommands.Add(key, new Queue<Command>());
		}
		if ((bool)this.entity.Invariable["thought_display_movement"])
		{
			float num = 8f;
			this.periodicMovement = new Sinusoid(0f, 12f, num, UnityEngine.Random.Range(0f, num));
		}
		if (this.entity.Invariable["display.position_offset"] != null)
		{
			this.displayOffsetScreen = (Vector3)this.entity.Invariable["display.position_offset"];
		}
		if (this.entity.Invariable.ContainsKey("display.texture_origin") && this.entity.Invariable["display.texture_origin"] != null)
		{
			this.textureOriginScreen = (Vector3)this.entity.Invariable["display.texture_origin"];
		}
		if (this.entity.Invariable.ContainsKey("display.mesh_name") && this.entity.Invariable["display.mesh_name"] != null)
		{
			if (this.entity.Invariable.ContainsKey("display.separate_tap") && this.entity.Invariable["display.separate_tap"] != null)
			{
				this.separateTap = (bool)this.entity.Invariable["display.separate_tap"];
			}
			this.hitMeshName = (string)this.entity.Invariable["display.mesh_name"];
			this.displayController = (this.entity.Invariable["display"] as IDisplayController).CloneWithHitMesh(simulation.EntityManager.DisplayControllerManager, this.hitMeshName, this.separateTap);
		}
		else
		{
			this.displayController = (this.entity.Invariable["display"] as IDisplayController).Clone(simulation.EntityManager.DisplayControllerManager);
		}
		this.displayController.Billboard(new BillboardDelegate(this.BillboardDelegate));
		if (this.entity.Invariable["thought_display"] != null)
		{
			if (this.entity.Invariable["thought_display.position_offset"] != null)
			{
				this.thoughtDisplayOffsetScreen = (Vector3)this.entity.Invariable["thought_display.position_offset"];
			}
			this.thoughtDisplayController = new DeferredDisplayController(this.entity.Invariable["thought_display"] as IDisplayController, simulation.EntityManager.DisplayControllerManager);
			this.thoughtDisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
			if (this.entity.Invariable["thought_mask_display"] != null)
			{
				this.thoughtMaskDisplayController = new DeferredDisplayController(this.entity.Invariable["thought_mask_display"] as IDisplayController, simulation.EntityManager.DisplayControllerManager);
				this.thoughtMaskDisplayController.Billboard(new BillboardDelegate(this.BillboardDelegate));
				if (this.entity.Invariable["thought_mask_display.position_offset"] != null)
				{
					this.thoughtMaskDisplayOffsetScreen = (Vector3)this.entity.Invariable["thought_mask_display.position_offset"];
				}
			}
			if (this.entity.Invariable["thought_item_bubble_display"] != null)
			{
				this.thoughtItemBubbleDisplayController = new DeferredDisplayController(this.entity.Invariable["thought_item_bubble_display"] as IDisplayController, simulation.EntityManager.DisplayControllerManager);
				this.thoughtItemBubbleDisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
				if (this.entity.Invariable["thought_item_bubble_display.position_offset"] != null)
				{
					this.thoughtItemBubbleDisplayOffsetScreen = (Vector3)this.entity.Invariable["thought_item_bubble_display.position_offset"];
				}
				if (TFPerfUtils.IsNonScalingDevice())
				{
					this.thoughtItemBubbleScalingMajor = new ConstantPattern(1f);
					this.thoughtItemBubbleScalingMinor = new ConstantPattern(1f);
				}
				else
				{
					this.thoughtItemBubbleScalingMajor = new Sinusoid(1f, 1.2f, 10f, UnityEngine.Random.Range(0f, 10f));
					this.thoughtItemBubbleScalingMinor = new Sinusoid(1f, 1.05f, UnityEngine.Random.Range(1f, 2f), 0f);
				}
			}
		}
		if (this.thoughtDisplayController != null && this.HasEntity<ResidentEntity>())
		{
			this.thoughtDisplayController.HitObject.Height -= 15f;
		}
		if (SBSettings.DebugDisplayControllers)
		{
			BasicSprite basicSprite = (this.entity.Invariable["debugBoxSprite"] as IDisplayController).Clone(simulation.EntityManager.DisplayControllerManager) as BasicSprite;
			basicSprite.Visible = true;
			basicSprite.Color = Color.magenta;
			basicSprite.Alpha = 0.2f;
			basicSprite.Resize(this.displayController.HitObject.Center, this.displayController.HitObject.Width, this.displayController.HitObject.Height);
			this.debugQuadHitBoxDisplayController = basicSprite;
			this.debugQuadHitBoxDisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
			if (this.thoughtDisplayController != null)
			{
				BasicSprite basicSprite2 = (this.entity.Invariable["debugBoxSprite"] as IDisplayController).Clone(simulation.EntityManager.DisplayControllerManager) as BasicSprite;
				basicSprite2.Visible = true;
				basicSprite2.Color = Color.red;
				basicSprite2.Alpha = 0.4f;
				basicSprite2.Resize(this.thoughtDisplayController.HitObject.Center, this.thoughtDisplayController.HitObject.Width, this.thoughtDisplayController.HitObject.Height);
				this.debugThoughtBoxDisplayController = basicSprite2;
				this.debugThoughtBoxDisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
			}
			BasicSprite basicSprite3 = (this.entity.Invariable["footprintSprite"] as IDisplayController).Clone(simulation.EntityManager.DisplayControllerManager) as BasicSprite;
			basicSprite3.Visible = true;
			basicSprite3.Color = Color.green;
			basicSprite3.Alpha = 0.4f;
			this.debugAlignedBoxDisplayController = basicSprite3;
		}
		if (this.entity.Invariable.ContainsKey("point_of_interest"))
		{
			this.pointOfInterestOffset = (Vector2)this.entity.Invariable["point_of_interest"];
		}
		if (this.entity.Invariable.ContainsKey("worker_spawner"))
		{
			this.workerSpawner = (bool)this.entity.Invariable["worker_spawner"];
		}
		if (this.entity.Invariable.ContainsKey("is_waypoint"))
		{
			this.isWaypoint = (bool)this.entity.Invariable["is_waypoint"];
		}
		if (this.entity.Invariable.ContainsKey("fx.producing.position_offset"))
		{
			this.particleDisplayOffsetScreen = (Vector3)this.entity.Invariable["fx.producing.position_offset"];
		}
		this.particleSystemRequestDelegate = new Simulated.ParticleSystemRequestDelegate(this);
		this.rewardParticleSystemRequestDelegate = new Simulated.RewardParticleRequestDelegate(this);
		this.thoughtBubblePopParticleRequestDelegate = new Simulated.ThoughtBubblePopParticleRequestDelegate(this);
		this.eatParticleRequestDelegate = new Simulated.EatParticleRequestDelegate(this);
		this.activateParticleSystemRequestDelegate = new Simulated.ActivateParticleRequestDelegate(this);
		this.dustParticleSystemRequestDelegate = new Simulated.SimulatedParticleRequestDelegate(this);
		this.starsParticleSystemRequestDelegate = new Simulated.SimulatedParticleRequestDelegate(this);
		this.simFlags = Simulated.SimulatedFlags.FIRST_ANIMATE;
		if (this.HasEntity<StructureDecorator>() && this.GetEntity<StructureDecorator>().Immobile)
		{
			this.simFlags |= Simulated.SimulatedFlags.BUILDING_ANIM_PATH;
		}
		else
		{
			this.simFlags |= Simulated.SimulatedFlags.MOBILE;
		}
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x0008DA70 File Offset: 0x0008BC70
	public void ClearPathInfo()
	{
		this.Variable["path"] = null;
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x060014F1 RID: 5361 RVA: 0x0008DA84 File Offset: 0x0008BC84
	public Identity Id
	{
		get
		{
			return this.entity.Id;
		}
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x060014F2 RID: 5362 RVA: 0x0008DA94 File Offset: 0x0008BC94
	public string Description
	{
		get
		{
			return string.Format("sim( {0}, name( {1} ) )", this.entity.Id, this.entity.Name);
		}
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x060014F3 RID: 5363 RVA: 0x0008DAC4 File Offset: 0x0008BCC4
	// (set) Token: 0x060014F4 RID: 5364 RVA: 0x0008DACC File Offset: 0x0008BCCC
	public bool Visible
	{
		get
		{
			return this.visible;
		}
		set
		{
			this.visible = value;
			this.displayController.Visible = this.visible;
			if (this.dropShadowDisplayController != null)
			{
				this.dropShadowDisplayController.Visible = this.visible;
				this.dropShadowDisplayController.Alpha = 0.7f;
			}
			if (SBSettings.DebugDisplayControllers)
			{
				this.debugQuadHitBoxDisplayController.Visible = (this.visible && this.debugHitBoxesVisible);
				this.debugAlignedBoxDisplayController.Visible = (this.visible && this.debugFootprintsVisible);
				if (this.debugAlignedBoxDisplayController.Visible)
				{
					this.UpdateDebugFootprint();
				}
				if (this.debugThoughtBoxDisplayController != null)
				{
					this.debugThoughtBoxDisplayController.Visible = (this.visible && this.debugHitBoxesVisible && this.thoughtDisplayController.GetDisplayState() != null);
				}
			}
		}
	}

	// Token: 0x170002DA RID: 730
	// (set) Token: 0x060014F5 RID: 5365 RVA: 0x0008DBBC File Offset: 0x0008BDBC
	public bool DebugHitBoxesVisible
	{
		set
		{
			if (SBSettings.DebugDisplayControllers)
			{
				this.debugHitBoxesVisible = value;
				this.Visible = this.visible;
			}
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x060014F6 RID: 5366 RVA: 0x0008DBDC File Offset: 0x0008BDDC
	// (set) Token: 0x060014F7 RID: 5367 RVA: 0x0008DBE4 File Offset: 0x0008BDE4
	public bool SimulatedQueryable
	{
		get
		{
			return this.simulatedQueryable;
		}
		set
		{
			this.simulatedQueryable = value;
		}
	}

	// Token: 0x170002DC RID: 732
	// (set) Token: 0x060014F8 RID: 5368 RVA: 0x0008DBF0 File Offset: 0x0008BDF0
	public bool DebugFootprintsVisible
	{
		set
		{
			if (SBSettings.DebugDisplayControllers)
			{
				this.debugFootprintsVisible = value;
				this.Visible = this.visible;
			}
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x060014F9 RID: 5369 RVA: 0x0008DC10 File Offset: 0x0008BE10
	// (set) Token: 0x060014FA RID: 5370 RVA: 0x0008DC20 File Offset: 0x0008BE20
	public float Alpha
	{
		get
		{
			return this.displayController.Alpha;
		}
		set
		{
			this.displayController.Alpha = value;
			if (this.thoughtDisplayController != null && this.thoughtDisplayController.Visible)
			{
				this.thoughtDisplayController.Alpha = value;
				if (this.thoughtMaskDisplayController != null && this.thoughtMaskDisplayController.Visible)
				{
					this.thoughtMaskDisplayController.Alpha = value;
				}
			}
			if (this.thoughtItemBubbleDisplayController != null && this.thoughtItemBubbleDisplayController.Visible)
			{
				this.thoughtItemBubbleDisplayController.Alpha = value;
			}
		}
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x060014FB RID: 5371 RVA: 0x0008DCB0 File Offset: 0x0008BEB0
	// (set) Token: 0x060014FC RID: 5372 RVA: 0x0008DCC0 File Offset: 0x0008BEC0
	public Color Color
	{
		get
		{
			return this.displayController.Color;
		}
		set
		{
			this.displayController.Color = value;
			if (this.thoughtDisplayController != null && this.thoughtDisplayController.Visible)
			{
				this.thoughtDisplayController.Color = value;
				if (this.thoughtMaskDisplayController != null && this.thoughtMaskDisplayController.Visible)
				{
					this.thoughtMaskDisplayController.Color = value;
				}
			}
			if (this.thoughtItemBubbleDisplayController != null && this.thoughtItemBubbleDisplayController.Visible)
			{
				this.thoughtItemBubbleDisplayController.Color = value;
			}
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x060014FD RID: 5373 RVA: 0x0008DD50 File Offset: 0x0008BF50
	private float Width
	{
		get
		{
			return this.displayController.Width;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x060014FE RID: 5374 RVA: 0x0008DD60 File Offset: 0x0008BF60
	private float Height
	{
		get
		{
			return this.displayController.Height;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x060014FF RID: 5375 RVA: 0x0008DD70 File Offset: 0x0008BF70
	public IDisplayController DisplayController
	{
		get
		{
			return this.displayController;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06001500 RID: 5376 RVA: 0x0008DD78 File Offset: 0x0008BF78
	public SBGUIShadowedLabel DynamicThinkingLabel
	{
		get
		{
			if (this.thinkingLabel == null)
			{
				this.thinkingLabel = (SBGUIShadowedLabel)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ThinkingTextLabel");
				this.thinkingLabel.name = this.Id + "_ThinkingLabel";
				this.thinkingLabel.SetParent(null);
			}
			this.thinkingLabel.SetActive(true);
			return this.thinkingLabel;
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06001501 RID: 5377 RVA: 0x0008DDE4 File Offset: 0x0008BFE4
	public SBGUIShadowedLabel DynamicThinkingSkipLabel
	{
		get
		{
			if (this.thinkingSkipLabel == null)
			{
				this.thinkingSkipLabel = (SBGUIShadowedLabel)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ThinkingTextLabel");
				this.thinkingSkipLabel.name = this.Id + "_ThinkingSkipLabel";
				this.thinkingSkipLabel.SetParent(null);
			}
			this.thinkingSkipLabel.SetActive(true);
			return this.thinkingSkipLabel;
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06001502 RID: 5378 RVA: 0x0008DE50 File Offset: 0x0008C050
	public SBGUIShadowedLabel DynamicThinkingSkipJjCounter
	{
		get
		{
			if (this.thinkingSkipJjCounter == null)
			{
				this.thinkingSkipJjCounter = (SBGUIShadowedLabel)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ThinkingTextLabel");
				this.thinkingSkipJjCounter.name = this.Id + "_ThinkingSkipCounter";
				this.thinkingSkipJjCounter.SetParent(null);
			}
			this.thinkingSkipJjCounter.SetActive(true);
			return this.thinkingSkipJjCounter;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06001503 RID: 5379 RVA: 0x0008DEBC File Offset: 0x0008C0BC
	public SBGUIAtlasImage DynamicThinkingIcon
	{
		get
		{
			if (this.thinkingIcon == null)
			{
				this.thinkingIcon = (SBGUIAtlasImage)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ThinkingIcon");
				this.thinkingIcon.name = this.Id + "_ThinkingIcon";
				this.thinkingIcon.SetParent(null);
			}
			this.thinkingIcon.SetActive(true);
			if (this.thinkingIcon.FindChild("UnavailableIcon") != null)
			{
				this.thinkingIcon.FindChild("UnavailableIcon").SetActive(this.showUnavailableIcon);
			}
			return this.thinkingIcon;
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06001504 RID: 5380 RVA: 0x0008DF60 File Offset: 0x0008C160
	public SBGUIButton ThinkingGhostButton
	{
		get
		{
			if (this.thinkingGhostButton == null)
			{
				this.thinkingGhostButton = (SBGUIButton)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ThinkingGhostButton");
				this.thinkingGhostButton.name = this.Id + "_ThinkingGhostButton";
				this.thinkingGhostButton.SetParent(null);
			}
			this.thinkingGhostButton.SetActive(true);
			this.thinkingGhostButton.SetVisible(false);
			return this.thinkingGhostButton;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06001505 RID: 5381 RVA: 0x0008DFD8 File Offset: 0x0008C1D8
	// (set) Token: 0x06001506 RID: 5382 RVA: 0x0008DFE8 File Offset: 0x0008C1E8
	public bool FootprintVisible
	{
		get
		{
			return this.footprintDisplayController != null;
		}
		set
		{
			if (value)
			{
				this.simFlags |= Simulated.SimulatedFlags.FORCE_ANIMATE_FOOTPRINT;
				float num;
				float num2;
				if (this.Flip)
				{
					AlignedBox alignedBox = this.entity.Invariable["footprint.flip"] as AlignedBox;
					num = alignedBox.Width;
					num2 = alignedBox.Height;
				}
				else
				{
					num = this.footprint.xmax;
					num2 = this.footprint.ymax;
				}
				if (Simulated.footprintDisplayControllerShared != null && !Simulated.footprintDisplayControllerShared.IsDestroyed)
				{
					((BasicSprite)Simulated.footprintDisplayControllerShared).Resize(Vector2.zero, num, num2);
				}
				else
				{
					Simulated.footprintDisplayControllerShared = new BasicSprite("Materials/unique/footprint", null, new Vector2(-0.5f * num, -0.5f * num2), num, num2);
					((BasicSprite)Simulated.footprintDisplayControllerShared).PublicInitialize();
				}
				Simulated.footprintDisplayControllerShared.Visible = true;
				this.footprintDisplayController = Simulated.footprintDisplayControllerShared;
			}
			else
			{
				this.simFlags &= ~Simulated.SimulatedFlags.FORCE_ANIMATE_FOOTPRINT;
				if (Simulated.footprintDisplayControllerShared != null)
				{
					Simulated.footprintDisplayControllerShared.Visible = false;
				}
				this.footprintDisplayController = null;
			}
		}
	}

	// Token: 0x170002E8 RID: 744
	// (set) Token: 0x06001507 RID: 5383 RVA: 0x0008E10C File Offset: 0x0008C30C
	public Color FootprintColor
	{
		set
		{
			if (this.footprintDisplayController != null)
			{
				this.footprintDisplayController.Color = value;
			}
		}
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x0008E128 File Offset: 0x0008C328
	private void UpdateAlignedBox()
	{
		this.box.xmin = this.position[1].x + this.footprint.xmin;
		this.box.xmax = this.position[1].x + this.footprint.xmax;
		this.box.ymin = this.position[1].y + this.footprint.ymin;
		this.box.ymax = this.position[1].y + this.footprint.ymax;
		this.snapBox.xmin = this.snapPosition.x + this.footprint.xmin;
		this.snapBox.xmax = this.snapPosition.x + this.footprint.xmax;
		this.snapBox.ymin = this.snapPosition.y + this.footprint.ymin;
		this.snapBox.ymax = this.snapPosition.y + this.footprint.ymax;
		if (this.scaffolding != null)
		{
			this.scaffolding.SetEnclosureBox(this.box);
		}
		if (this.fence != null)
		{
			this.fence.SetEnclosureBox(this.box);
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06001509 RID: 5385 RVA: 0x0008E298 File Offset: 0x0008C498
	// (set) Token: 0x0600150A RID: 5386 RVA: 0x0008E2A0 File Offset: 0x0008C4A0
	private string StateModifierString
	{
		get
		{
			return this.mStateModifierString;
		}
		set
		{
			this.mStateModifierString = value;
		}
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x0008E2AC File Offset: 0x0008C4AC
	public string UseStateModifierString(string state)
	{
		if (this.mStateModifierString == null)
		{
			return state;
		}
		return string.Format(this.mStateModifierString, state);
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x0008E2D4 File Offset: 0x0008C4D4
	private void UpdateDebugFootprint()
	{
		if (SBSettings.DebugDisplayControllers && (this.debugHitBoxesVisible || this.debugFootprintsVisible))
		{
			BasicSprite basicSprite = this.debugAlignedBoxDisplayController as BasicSprite;
			if (basicSprite != null)
			{
				if (this.Flip)
				{
					AlignedBox alignedBox = this.entity.Invariable["footprint.flip"] as AlignedBox;
					basicSprite.Resize(new Vector2(-0.5f * alignedBox.Width, -0.5f * alignedBox.Height), alignedBox.Width, alignedBox.Height);
				}
				else
				{
					AlignedBox alignedBox2 = this.entity.Invariable["footprint"] as AlignedBox;
					basicSprite.Resize(new Vector2(-0.5f * alignedBox2.Width, -0.5f * alignedBox2.Height), alignedBox2.Width, alignedBox2.Height);
				}
				this.simFlags |= Simulated.SimulatedFlags.FORCE_ANIMATE_FOOTPRINT;
			}
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x0600150D RID: 5389 RVA: 0x0008E3C8 File Offset: 0x0008C5C8
	// (set) Token: 0x0600150E RID: 5390 RVA: 0x0008E3DC File Offset: 0x0008C5DC
	public Vector2 Position
	{
		get
		{
			return this.position[1];
		}
		set
		{
			this.position[0] = this.position[1];
			this.position[1] = value;
			this.snapPosition = value;
			this.UpdateAlignedBox();
		}
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x0600150F RID: 5391 RVA: 0x0008E42C File Offset: 0x0008C62C
	public Vector2 PositionCenter
	{
		get
		{
			return new Vector2(this.position[1].x + this.footprint.Width / 2f, this.position[1].y + this.footprint.Height / 2f);
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06001510 RID: 5392 RVA: 0x0008E484 File Offset: 0x0008C684
	// (set) Token: 0x06001511 RID: 5393 RVA: 0x0008E48C File Offset: 0x0008C68C
	public Vector2 SnapPosition
	{
		get
		{
			return this.snapPosition;
		}
		set
		{
			this.snapPosition = value;
			this.UpdateAlignedBox();
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06001512 RID: 5394 RVA: 0x0008E49C File Offset: 0x0008C69C
	public Vector2 ThoughtDisplayOffsetScreen
	{
		get
		{
			return this.thoughtDisplayOffsetScreen;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06001513 RID: 5395 RVA: 0x0008E4AC File Offset: 0x0008C6AC
	public Vector3 ThoughtDisplayOffsetWorld
	{
		get
		{
			TFUtils.Assert(this.thoughtDisplayOffsetWorld != null, "You should calculate the world offset before trying to call this property!");
			return this.thoughtDisplayOffsetWorld.GetValueOrDefault();
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06001514 RID: 5396 RVA: 0x0008E4DC File Offset: 0x0008C6DC
	public Vector3 DisplayOffsetWorld
	{
		get
		{
			Vector3? vector = this.displayOffsetWorld;
			if (vector != null)
			{
				return this.displayOffsetWorld.Value;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06001515 RID: 5397 RVA: 0x0008E510 File Offset: 0x0008C710
	public Vector3 TextureOriginWorld
	{
		get
		{
			Vector3? vector = this.textureOriginWorld;
			if (vector != null)
			{
				return this.textureOriginWorld.Value;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06001516 RID: 5398 RVA: 0x0008E544 File Offset: 0x0008C744
	public Vector2 ThoughtMaskDisplayOffsetScreen
	{
		get
		{
			return this.thoughtMaskDisplayOffsetScreen;
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06001517 RID: 5399 RVA: 0x0008E554 File Offset: 0x0008C754
	public Vector3 ThoughtMaskDisplayOffsetWorld
	{
		get
		{
			TFUtils.Assert(this.thoughtMaskDisplayOffsetWorld != null, "You should calculate the world offset before trying to call this property!");
			return this.thoughtMaskDisplayOffsetWorld.GetValueOrDefault();
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06001518 RID: 5400 RVA: 0x0008E584 File Offset: 0x0008C784
	public IDisplayController ThoughtDisplayController
	{
		get
		{
			return this.thoughtDisplayController;
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06001519 RID: 5401 RVA: 0x0008E58C File Offset: 0x0008C78C
	public IDisplayController ThoughtMaskDisplayController
	{
		get
		{
			return this.thoughtMaskDisplayController;
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x0600151A RID: 5402 RVA: 0x0008E594 File Offset: 0x0008C794
	public Vector2 PointOfInterest
	{
		get
		{
			return this.Position + this.pointOfInterestOffset;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x0600151B RID: 5403 RVA: 0x0008E5A8 File Offset: 0x0008C7A8
	public bool WorkerSpawner
	{
		get
		{
			return this.workerSpawner;
		}
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x0600151C RID: 5404 RVA: 0x0008E5B0 File Offset: 0x0008C7B0
	public bool IsWaypoint
	{
		get
		{
			return this.isWaypoint;
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x0600151D RID: 5405 RVA: 0x0008E5B8 File Offset: 0x0008C7B8
	public AlignedBox Box
	{
		get
		{
			return this.box;
		}
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x0600151E RID: 5406 RVA: 0x0008E5C0 File Offset: 0x0008C7C0
	public AlignedBox SnapBox
	{
		get
		{
			return this.snapBox;
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x0600151F RID: 5407 RVA: 0x0008E5C8 File Offset: 0x0008C7C8
	public AlignedBox Footprint
	{
		get
		{
			return this.footprint;
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06001520 RID: 5408 RVA: 0x0008E5D0 File Offset: 0x0008C7D0
	public InteractionState InteractionState
	{
		get
		{
			return this.interactionState;
		}
	}

	// Token: 0x06001521 RID: 5409 RVA: 0x0008E5D8 File Offset: 0x0008C7D8
	public void AddClickListener(Action handler)
	{
		this.clickListeners.Add(handler);
	}

	// Token: 0x06001522 RID: 5410 RVA: 0x0008E5E8 File Offset: 0x0008C7E8
	public bool RemoveClickListener(Action handler)
	{
		return this.clickListeners.Remove(handler);
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06001523 RID: 5411 RVA: 0x0008E5F8 File Offset: 0x0008C7F8
	public List<Action> ClickListeners
	{
		get
		{
			return new List<Action>(this.clickListeners.ToArray());
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06001524 RID: 5412 RVA: 0x0008E60C File Offset: 0x0008C80C
	public int SelectionPriority
	{
		get
		{
			int num = this.SelectionPriorityBaggage;
			int num2 = Simulated.prioritizedActions.IndexOf(this.action);
			if (num2 == -1)
			{
				num2 = Simulated.prioritizedActions.Count;
			}
			if (!this.interactionState.IsSelectable && !this.interactionState.HasClickCommandFunctionality)
			{
				num += Simulated.prioritizedActions.Count;
			}
			return num2 + num;
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06001525 RID: 5413 RVA: 0x0008E674 File Offset: 0x0008C874
	// (set) Token: 0x06001526 RID: 5414 RVA: 0x0008E67C File Offset: 0x0008C87C
	public int SelectionPriorityBaggage
	{
		get
		{
			return this.selectionPriorityBaggage;
		}
		set
		{
			this.selectionPriorityBaggage = value;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06001527 RID: 5415 RVA: 0x0008E688 File Offset: 0x0008C888
	public int TemptationPriority
	{
		get
		{
			return -1 * (Simulated.priorityOrder.IndexOf(this.action) + 1);
		}
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x0008E6A0 File Offset: 0x0008C8A0
	protected bool IntersectsFootprint(Ray ray)
	{
		Vector3 zero = Vector3.zero;
		zero.x = this.Position.x;
		zero.y = this.Position.y;
		Plane plane = new Plane(Vector3.forward, zero);
		float d;
		if (plane.Raycast(ray, out d))
		{
			Vector3 vector = ray.origin + ray.direction * d;
			return this.box.xmin < vector.x && vector.x < this.box.xmax && this.box.ymin < vector.y && vector.y < this.box.ymax;
		}
		return false;
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x0008E774 File Offset: 0x0008C974
	public bool Intersects(Ray ray)
	{
		return this.displayController.Intersects(ray) || (this.thoughtDisplayController != null && this.thoughtDisplayController.GetDisplayState() != null && this.thoughtDisplayController.Visible && this.thoughtDisplayController.Intersects(ray)) || (this.useFootprintIntersection && this.IntersectsFootprint(ray));
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x0600152A RID: 5418 RVA: 0x0008E7E8 File Offset: 0x0008C9E8
	// (set) Token: 0x0600152B RID: 5419 RVA: 0x0008E7F0 File Offset: 0x0008C9F0
	public bool Flip
	{
		get
		{
			return this.displayControllerFlipped;
		}
		set
		{
			this.displayControllerFlipped = value;
			string displayState = this.displayController.GetDisplayState();
			int num = displayState.LastIndexOf('.');
			string state = displayState.Substring(0, (num < 0) ? displayState.Length : num);
			if (value)
			{
				this.displayController.DefaultDisplayState = "default.flip";
				if (this.entity.Invariable.ContainsKey("display.default.flip.position_offset") && this.entity.Invariable["display.default.flip.position_offset"] != null)
				{
					this.displayOffsetScreen = (Vector3)this.entity.Invariable["display.default.flip.position_offset"];
				}
				this.displayControllerExtension = ".flip";
				this.footprint = (AlignedBox)this.entity.Invariable["footprint.flip"];
			}
			else
			{
				if (this.entity.Invariable["display.position_offset"] != null)
				{
					this.displayOffsetScreen = (Vector3)this.entity.Invariable["display.position_offset"];
				}
				this.displayController.DefaultDisplayState = "default";
				this.displayControllerExtension = string.Empty;
				this.footprint = (AlignedBox)this.entity.Invariable["footprint"];
			}
			this.DisplayState(state);
			this.UpdateAlignedBox();
			this.UpdateDebugFootprint();
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x0600152C RID: 5420 RVA: 0x0008E958 File Offset: 0x0008CB58
	public ReadOnlyIndexer Invariable
	{
		get
		{
			return this.entity.Invariable;
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x0600152D RID: 5421 RVA: 0x0008E968 File Offset: 0x0008CB68
	public ReadWriteIndexer Variable
	{
		get
		{
			return this.entity.Variable;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x0600152E RID: 5422 RVA: 0x0008E978 File Offset: 0x0008CB78
	// (set) Token: 0x0600152F RID: 5423 RVA: 0x0008E980 File Offset: 0x0008CB80
	public bool IsSwarmManaged
	{
		get
		{
			return this.swarmManaged;
		}
		set
		{
			this.swarmManaged = value;
		}
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x0008E98C File Offset: 0x0008CB8C
	public void LoadInitialState(Simulated.StateAction action)
	{
		this.action = action;
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x0008E998 File Offset: 0x0008CB98
	public void EnterInitialState(Simulated.StateAction action, Simulation simulation)
	{
		this.action = action;
		this.action.Enter(simulation, this);
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x0008E9B0 File Offset: 0x0008CBB0
	public void Push(Command command)
	{
		this.commands.Enqueue(command);
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06001533 RID: 5427 RVA: 0x0008E9C0 File Offset: 0x0008CBC0
	public Entity Entity
	{
		get
		{
			return this.entity;
		}
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x0008E9C8 File Offset: 0x0008CBC8
	public bool HasEntity<T>() where T : EntityDecorator
	{
		return this.entity.HasDecorator<T>();
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x0008E9D8 File Offset: 0x0008CBD8
	public T GetEntity<T>() where T : EntityDecorator
	{
		return this.entity.GetDecorator<T>();
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x0008E9E8 File Offset: 0x0008CBE8
	public void SetFootprint(Simulation simulation, bool enable = true)
	{
		Terrain terrain = (simulation == null) ? null : simulation.Terrain;
		bool flag = this.HasEntity<StructureDecorator>() && this.GetEntity<StructureDecorator>().IsObstacle;
		if (terrain != null && this.box.xmin >= 0f && flag)
		{
			terrain.SetOrClearObstacle(this.box, enable);
			simulation.ResetAllAffectedPaths(this.box);
		}
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x0008EA5C File Offset: 0x0008CC5C
	public void Warp(Vector2 position, Simulation simulation = null)
	{
		this.Position = position;
		this.Position = position;
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x0008EA6C File Offset: 0x0008CC6C
	public void FlipWarp(Simulation simulation = null)
	{
		if ((this.simFlags & Simulated.SimulatedFlags.FORCE_ANIMATE_FOOTPRINT) != (Simulated.SimulatedFlags)0)
		{
			this.simFlags = (Simulated.SimulatedFlags.BUILDING_ANIM_PATH | Simulated.SimulatedFlags.FORCE_ANIMATE_FOOTPRINT);
			if (this.footprintDisplayController != null)
			{
				this.footprintDisplayController.Position = this.snapPosition + new Vector2(this.footprintDisplayController.Width * 0.5f, this.footprintDisplayController.Height * 0.5f);
			}
		}
		else
		{
			this.simFlags = Simulated.SimulatedFlags.BUILDING_ANIM_PATH;
		}
		this.displayOffsetWorld = new Vector3?(this.CameraOffsetToWorldVector(this.displayOffsetScreen, simulation.TheCamera));
		Vector3 b = new Vector3((this.box.xmax + this.box.xmin) * 0.5f, (this.box.ymax + this.box.ymin) * 0.5f, 0f);
		this.displayController.Position = this.displayOffsetWorld.Value + b;
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x0008EB68 File Offset: 0x0008CD68
	public void AddScaffolding(Simulation simulation)
	{
		if (this.scaffolding == null)
		{
			this.scaffolding = simulation.enclosureManager.AddScaffolding(this.Box, new BillboardDelegate(SBCamera.BillboardDefinition));
		}
		if (!this.scaffolding.IsInitialized())
		{
			this.scaffolding = null;
		}
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x0008EBBC File Offset: 0x0008CDBC
	public void RemoveScaffolding(Simulation simulation)
	{
		if (this.scaffolding == null)
		{
			return;
		}
		simulation.enclosureManager.RemoveScaffolding(this.scaffolding);
		this.scaffolding = null;
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x0008EBF0 File Offset: 0x0008CDF0
	public void AddFence(Simulation simulation)
	{
		if (this.fence == null)
		{
			this.fence = simulation.enclosureManager.AddFence(this.Box, new BillboardDelegate(SBCamera.BillboardDefinition));
		}
		if (!this.fence.IsInitialized())
		{
			this.fence = null;
		}
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x0008EC44 File Offset: 0x0008CE44
	public void RemoveFence(Simulation simulation)
	{
		if (this.fence == null)
		{
			return;
		}
		simulation.enclosureManager.RemoveFence(this.fence);
		this.fence = null;
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x0008EC78 File Offset: 0x0008CE78
	public bool Simulate(Simulation simulation, Session session)
	{
		if (this.command == null && this.commands.Count != 0)
		{
			this.command = this.commands.Dequeue();
			Simulated.StateAction key;
			if (this.machine.Transition(this.action, this.command.Type, out key))
			{
				Simulated.StateAction stateAction = this.action;
				this.action.Leave(simulation, this);
				this.action = key;
				this.command.TryExecuteOnComplete();
				this.action.Enter(simulation, this);
			}
			else if (this.machine.Delegate(this.action, this.command.Type, out key))
			{
				this.delegatedCommands[key].Enqueue(this.command);
			}
			else
			{
				this.command.TryExecuteOnComplete();
				this.command = null;
			}
		}
		return this.action.Simulate(simulation, this, session);
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x0008ED70 File Offset: 0x0008CF70
	public void UpdateControls(Simulation simulation)
	{
		if (this.action != null && this.action is Simulated.StateActionBuildingDefault)
		{
			(this.action as Simulated.StateActionBuildingDefault).UpdateControls(simulation, this);
		}
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x0008EDA0 File Offset: 0x0008CFA0
	public void DestroyDisplayControllers()
	{
		this.displayController.Destroy();
		if (this.thoughtDisplayController != null)
		{
			this.thoughtDisplayController.Destroy();
			if (this.thoughtMaskDisplayController != null)
			{
				this.thoughtMaskDisplayController.Destroy();
			}
		}
		if (this.thoughtItemBubbleDisplayController != null)
		{
			this.thoughtItemBubbleDisplayController.Destroy();
		}
		if (this.dropShadowDisplayController != null)
		{
			this.dropShadowDisplayController.Destroy();
		}
		if (SBSettings.DebugDisplayControllers)
		{
			this.debugQuadHitBoxDisplayController.Destroy();
			this.debugAlignedBoxDisplayController.Destroy();
			if (this.debugThoughtBoxDisplayController != null)
			{
				this.debugThoughtBoxDisplayController.Destroy();
			}
		}
		if (this.thinkingIcon != null)
		{
			UnityEngine.Object.Destroy(this.thinkingIcon.gameObject);
			this.thinkingIcon = null;
		}
		if (this.thinkingSkipLabel != null)
		{
			UnityEngine.Object.Destroy(this.thinkingSkipLabel.gameObject);
			this.thinkingSkipLabel = null;
		}
		if (this.thinkingSkipJjCounter != null)
		{
			UnityEngine.Object.Destroy(this.thinkingSkipJjCounter.gameObject);
			this.thinkingSkipJjCounter = null;
		}
		if (this.thinkingLabel != null)
		{
			UnityEngine.Object.Destroy(this.thinkingLabel.gameObject);
			this.thinkingLabel = null;
		}
		if (this.thinkingGhostButton != null)
		{
			UnityEngine.Object.Destroy(this.thinkingGhostButton.gameObject);
			this.thinkingGhostButton = null;
		}
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x0008EF10 File Offset: 0x0008D110
	public void Destroy(Simulation simulation)
	{
		TFUtils.DebugLog("Responding to Destroy Command with default action for Simulated(" + this.Id.Describe() + ")");
		this.DestroyDisplayControllers();
		simulation.particleSystemManager.RemoveRequestWithDelegate(this.particleSystemRequestDelegate);
		simulation.particleSystemManager.RemoveRequestWithDelegate(this.activateParticleSystemRequestDelegate);
		simulation.particleSystemManager.RemoveRequestWithDelegate(this.rewardParticleSystemRequestDelegate);
		simulation.particleSystemManager.RemoveRequestWithDelegate(this.dustParticleSystemRequestDelegate);
		simulation.particleSystemManager.RemoveRequestWithDelegate(this.starsParticleSystemRequestDelegate);
		this.command = null;
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x0008EFA0 File Offset: 0x0008D1A0
	public void FirstAnimate(Simulation simulation)
	{
		if (this.displayOffsetWorld == null)
		{
			this.displayOffsetWorld = new Vector3?(this.CameraOffsetToWorldVector(this.displayOffsetScreen, simulation.TheCamera));
		}
		if ((this.simFlags & Simulated.SimulatedFlags.BUILDING_ANIM_PATH) != (Simulated.SimulatedFlags)0 && this.textureOriginWorld == null)
		{
			if (this.textureOriginScreen != Vector3.zero)
			{
				this.textureOriginWorld = new Vector3?(this.CameraOffsetToWorldVector(this.textureOriginScreen, simulation.TheCamera));
			}
			else
			{
				this.textureOriginWorld = new Vector3?(Vector3.zero);
			}
		}
		if (this.thoughtDisplayController != null)
		{
			Vector3 vector = -simulation.TheCamera.transform.forward * 1f;
			if (this.thoughtDisplayOffsetWorld == null)
			{
				this.thoughtDisplayOffsetWorld = new Vector3?(this.CameraOffsetToWorldVector(this.thoughtDisplayOffsetScreen, simulation.TheCamera));
			}
			if (this.thoughtItemBubbleDisplayController != null && this.thoughtItemBubbleDisplayOffsetWorld == null)
			{
				this.thoughtItemBubbleDisplayOffsetWorld = new Vector3?(this.CameraOffsetToWorldVector(this.thoughtItemBubbleDisplayOffsetScreen, simulation.TheCamera));
				Vector3? vector2 = this.thoughtItemBubbleDisplayOffsetWorld;
				this.thoughtItemBubbleDisplayOffsetWorld = ((vector2 == null) ? null : new Vector3?(vector2.Value + vector));
			}
			if (this.thoughtMaskDisplayOffsetWorld == null)
			{
				this.thoughtMaskDisplayOffsetWorld = new Vector3?(this.CameraOffsetToWorldVector(this.thoughtMaskDisplayOffsetScreen, simulation.TheCamera));
				Vector3? vector3 = this.thoughtMaskDisplayOffsetWorld;
				this.thoughtMaskDisplayOffsetWorld = ((vector3 == null) ? null : new Vector3?(vector3.Value + vector * 2f));
			}
		}
		this.simFlags &= ~Simulated.SimulatedFlags.FIRST_ANIMATE;
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x0008F184 File Offset: 0x0008D384
	public void EnableAnimateAction(bool enable)
	{
		if (enable)
		{
			this.simFlags |= Simulated.SimulatedFlags.FORCE_ANIMATE_ACTION;
		}
		else
		{
			this.simFlags &= ~Simulated.SimulatedFlags.FORCE_ANIMATE_ACTION;
			Vector3 b = new Vector3((this.box.xmax + this.box.xmin) * 0.5f, (this.box.ymax + this.box.ymin) * 0.5f, 0f);
			this.displayController.Position = this.displayOffsetWorld.Value + b - this.textureOriginWorld.Value;
		}
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x0008F22C File Offset: 0x0008D42C
	public void Animate(Simulation simulation)
	{
		if ((this.simFlags & Simulated.SimulatedFlags.BUILDING_ANIM_PATH) != (Simulated.SimulatedFlags)0)
		{
			Vector3 b = Vector3.zero;
			if ((this.simFlags & Simulated.SimulatedFlags.FORCE_ANIMATE_ACTION) != (Simulated.SimulatedFlags)0 && this.action is Simulated.Animated)
			{
				b = ((Simulated.Animated)this.action).Animate(simulation, this);
			}
			if ((this.simFlags & Simulated.SimulatedFlags.FORCE_ANIMATE_FOOTPRINT) != (Simulated.SimulatedFlags)0 && this.footprintDisplayController != null)
			{
				this.footprintDisplayController.Position = this.snapPosition + new Vector2(this.footprintDisplayController.Width * 0.5f, this.footprintDisplayController.Height * 0.5f);
			}
			Vector3 b2 = new Vector3((this.box.xmax + this.box.xmin) * 0.5f, (this.box.ymax + this.box.ymin) * 0.5f, 0f);
			this.displayController.Position = this.displayOffsetWorld.Value + b2 - this.textureOriginWorld.Value + b;
		}
		else
		{
			Vector2 vector = (this.position[1] - this.position[0]) * simulation.Interpolant + this.position[0];
			Vector3 a = new Vector3(vector.x, vector.y, 0f);
			this.displayController.Position = a + this.displayOffsetWorld.Value;
			if (this.dropShadowDisplayController != null)
			{
				this.dropShadowDisplayController.Position = a + this.displayOffsetWorld.Value;
			}
		}
		if ((this.simFlags & Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE) != (Simulated.SimulatedFlags)0)
		{
			this.AnimateBounce(simulation);
		}
		if ((this.simFlags & Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE_START) != (Simulated.SimulatedFlags)0)
		{
			this.AnimateBounceStart(simulation);
		}
		if ((this.simFlags & Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE_END) != (Simulated.SimulatedFlags)0)
		{
			this.AnimateBounceEnd(simulation);
		}
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x0008F43C File Offset: 0x0008D63C
	public void Bounce()
	{
		this.simFlags |= Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE;
		this.bounceStartTime = DateTime.Now;
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x0008F458 File Offset: 0x0008D658
	public void BounceStart()
	{
		this.simFlags |= Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE_START;
		this.bounceStartStartTime = DateTime.Now;
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x0008F474 File Offset: 0x0008D674
	public void BounceEnd()
	{
		this.simFlags |= Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE_END;
		this.bounceEndStartTime = DateTime.Now;
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x0008F494 File Offset: 0x0008D694
	public void BounceCleanup()
	{
		if (this.calledBounceStart)
		{
			this.BounceEnd();
		}
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x0008F4A8 File Offset: 0x0008D6A8
	public void AnimateBounce(Simulation simulation)
	{
		float num = (float)(DateTime.Now - this.bounceStartTime).TotalSeconds;
		this.AnimateScaleAndFlip(simulation.bounceEndInterpolator.GetHermiteAtTime(num));
		if (num > simulation.bounceInterpolator.MaxTime)
		{
			this.simFlags &= ~Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE;
		}
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x0008F504 File Offset: 0x0008D704
	public void AnimateBounceStart(Simulation simulation)
	{
		float num = (float)(DateTime.Now - this.bounceStartStartTime).TotalSeconds;
		this.calledBounceStart = true;
		this.AnimateScaleAndFlip(simulation.bounceStartInterpolator.GetHermiteAtTime(num));
		if (num > simulation.bounceStartInterpolator.MaxTime)
		{
			this.simFlags &= ~Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE_START;
		}
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x0008F564 File Offset: 0x0008D764
	public void AnimateBounceEnd(Simulation simulation)
	{
		float num = (float)(DateTime.Now - this.bounceEndStartTime).TotalSeconds;
		this.calledBounceStart = false;
		this.AnimateScaleAndFlip(simulation.bounceEndInterpolator.GetHermiteAtTime(num));
		if (num > simulation.bounceEndInterpolator.MaxTime)
		{
			this.simFlags &= ~Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE_END;
		}
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x0008F5C8 File Offset: 0x0008D7C8
	public void AnimateScaleAndFlip(Vector3 scale)
	{
		if (!TFPerfUtils.IsNonScalingDevice())
		{
			if (this.Flip)
			{
				scale.x *= -1f;
			}
			this.displayController.Scale = scale;
		}
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x0008F60C File Offset: 0x0008D80C
	public void AnimateDebugHitBox(Simulation simulation)
	{
		if (SBSettings.DebugDisplayControllers && (this.debugHitBoxesVisible || this.debugFootprintsVisible))
		{
			this.debugQuadHitBoxDisplayController.Position = this.displayController.Position;
			this.debugQuadHitBoxDisplayController.OnUpdate(simulation.TheCamera, null);
			Vector2 vector = (this.position[1] - this.position[0]) * simulation.Interpolant + this.position[0];
			Vector3 vector2 = new Vector3(vector.x, vector.y, 0f);
			this.debugAlignedBoxDisplayController.Position = vector2;
			if (this.debugThoughtBoxDisplayController != null && this.thoughtDisplayController.GetDisplayState() != null)
			{
				this.debugThoughtBoxDisplayController.Position = this.thoughtDisplayController.Position;
				this.debugThoughtBoxDisplayController.OnUpdate(simulation.TheCamera, null);
			}
		}
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x0008F714 File Offset: 0x0008D914
	public void AnimateOtherControllers(Simulation simulation)
	{
		TFUtils.Assert(this.thoughtDisplayOffsetWorld != null, "Need to set thoughtDisplayOffsetWorld before calling AnimateOtherControllers!");
		float atTime = (float)simulation.Time;
		float z = (this.periodicMovement == null) ? 0f : this.periodicMovement.ValueAtTime(atTime);
		Vector3 a = this.displayController.Position + this.thoughtDisplayOffsetWorld.Value + new Vector3(0f, 0f, z);
		this.thoughtDisplayController.Position = a;
		if (this.thoughtMaskDisplayController != null)
		{
			this.thoughtMaskDisplayController.Position = a + this.thoughtMaskDisplayOffsetWorld.Value;
			DisplayControllerFlags flags = this.thoughtMaskDisplayController.Flags;
			if ((flags & (DisplayControllerFlags.SWITCHED_STATE | DisplayControllerFlags.NEED_UPDATE)) != (DisplayControllerFlags)0)
			{
				this.thoughtMaskDisplayController.OnUpdate(simulation.TheCamera, simulation.particleSystemManager);
				this.thoughtMaskDisplayController.Flags = (flags & ~DisplayControllerFlags.SWITCHED_STATE);
			}
		}
		TFUtils.Assert(this.thoughtItemBubbleDisplayController != null, "Simulateds are all assumed to have thoughtItemBubbleDisplayController");
		DisplayControllerFlags flags2 = this.thoughtItemBubbleDisplayController.Flags;
		if ((flags2 & DisplayControllerFlags.VISIBLE_AND_VALID_STATE) != (DisplayControllerFlags)0)
		{
			this.thoughtItemBubbleDisplayController.Position = a + this.thoughtItemBubbleDisplayOffsetWorld.Value;
			if (this.periodicMovement != null)
			{
				float num = this.thoughtItemBubbleScalingMajor.ValueAtTime(atTime);
				float num2 = this.thoughtItemBubbleScalingMinor.ValueAtTime(atTime);
				float num3 = num * num2;
				this.thoughtItemBubbleDisplayController.Scale = new Vector3(num3, num3, num3);
			}
			if ((flags2 & (DisplayControllerFlags.SWITCHED_STATE | DisplayControllerFlags.NEED_UPDATE)) != (DisplayControllerFlags)0)
			{
				this.thoughtItemBubbleDisplayController.OnUpdate(simulation.TheCamera, simulation.particleSystemManager);
				this.thoughtItemBubbleDisplayController.Flags = (flags2 & ~DisplayControllerFlags.SWITCHED_STATE);
			}
		}
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x0008F8BC File Offset: 0x0008DABC
	public void DisplayState(string state)
	{
		state = this.UseStateModifierString(state);
		string text = (state == null) ? null : (state + this.displayControllerExtension);
		this.displayController.DisplayState(text);
		string property = "display." + text + ".mesh_name";
		if (this.entity.Invariable.ContainsKey(property) && this.entity.Invariable[property] != null)
		{
			this.hitMeshName = (string)this.entity.Invariable[property];
			this.displayController.ChangeMesh(text, this.hitMeshName);
		}
		if (this.dropShadowDisplayController != null)
		{
			this.dropShadowDisplayController.Visible = (state != null);
		}
		if (SBSettings.DebugDisplayControllers)
		{
			this.debugQuadHitBoxDisplayController.Visible = (state != null && this.debugHitBoxesVisible);
			this.debugAlignedBoxDisplayController.Visible = (state != null && this.debugFootprintsVisible);
			if (this.debugThoughtBoxDisplayController != null)
			{
				this.debugThoughtBoxDisplayController.Visible = (this.debugQuadHitBoxDisplayController.Visible && this.thoughtDisplayController.GetDisplayState() != null);
			}
		}
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x0008F9F8 File Offset: 0x0008DBF8
	public string GetDisplayState()
	{
		return this.displayController.GetDisplayState();
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x0008FA08 File Offset: 0x0008DC08
	public void DisplayThoughtState(string state, Simulation simulation)
	{
		string displayState = this.thoughtItemBubbleDisplayController.GetDisplayState();
		string str = "thought_display." + state;
		this.thoughtDisplayController.DisplayState(state);
		if (state != null && displayState == null)
		{
			this.FirstAnimate(simulation);
			this.AnimateOtherControllers(simulation);
		}
		string property = str + ".position_offset";
		if (!this.entity.Invariable.ContainsKey(property))
		{
			property = "thought_display.default.position_offset";
		}
		if (this.entity.Invariable.ContainsKey(property))
		{
			Vector3 rhs = (Vector3)this.entity.Invariable[property];
			if (this.thoughtDisplayOffsetScreen != rhs)
			{
				this.thoughtDisplayOffsetScreen = rhs;
				this.thoughtDisplayOffsetWorld = new Vector3?(this.CameraOffsetToWorldVector(this.thoughtDisplayOffsetScreen, simulation.TheCamera));
			}
		}
		if (this.thoughtMaskDisplayController != null)
		{
			this.thoughtMaskDisplayController.DisplayState(state);
		}
		if (this.thoughtItemBubbleDisplayController != null)
		{
			this.thoughtItemBubbleDisplayController.DisplayState(state);
		}
		if (SBSettings.DebugDisplayControllers && this.debugThoughtBoxDisplayController != null)
		{
			this.debugThoughtBoxDisplayController.Visible = (this.debugQuadHitBoxDisplayController.Visible && this.thoughtDisplayController.GetDisplayState() != null);
		}
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x0008FB50 File Offset: 0x0008DD50
	public void DisplayThoughtState(string overrideSubjectMaterial, string state, Simulation simulation)
	{
		this.DisplayThoughtState(state, simulation);
		TFUtils.Assert(overrideSubjectMaterial != null, "Cannot set thought display's subject material to null");
		this.thoughtDisplayController.UpdateMaterialOrTexture(overrideSubjectMaterial);
		this.DisplayThoughtItemBubbleState(state, simulation);
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x0008FB8C File Offset: 0x0008DD8C
	public void RemoveDynamicThinkingElements()
	{
		if (this.thinkingGhostButton != null)
		{
			this.thinkingGhostButton.ClearClickEvents();
		}
		Action<SBGUIElement> action = delegate(SBGUIElement el)
		{
			if (el != null)
			{
				el.SetParent(null);
				el.SetActive(false);
				UnityEngine.Object.Destroy(el.gameObject);
			}
		};
		action(this.thinkingIcon);
		action(this.thinkingLabel);
		action(this.thinkingSkipLabel);
		action(this.thinkingSkipJjCounter);
		action(this.thinkingGhostButton);
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x0008FC10 File Offset: 0x0008DE10
	public void DisplayThoughtItemBubbleState(string state, Simulation simulation)
	{
		if (this.thoughtItemBubbleDisplayController != null)
		{
			string displayState = this.thoughtItemBubbleDisplayController.GetDisplayState();
			this.thoughtItemBubbleDisplayController.DisplayState(state);
			if (state != null && displayState == null)
			{
				this.FirstAnimate(simulation);
				this.AnimateOtherControllers(simulation);
			}
		}
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x0008FC5C File Offset: 0x0008DE5C
	public void SetCostume(CostumeManager.Costume costume)
	{
		Paperdoll paperdoll = this.displayController as Paperdoll;
		if (paperdoll != null)
		{
			paperdoll.ApplyCostumeWithLOD(costume, this.entity.DefinitionId);
		}
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x0008FC90 File Offset: 0x0008DE90
	public void AddPendingCommand(Simulated.PendingCommand pc)
	{
		this.pendingCommands.Add(pc);
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x0008FCA0 File Offset: 0x0008DEA0
	public void ClearPendingCommands()
	{
		this.pendingCommands.Clear();
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x0008FCB0 File Offset: 0x0008DEB0
	public void SendPendingCommands(Simulation simulation)
	{
		foreach (Simulated.PendingCommand pendingCommand in this.pendingCommands)
		{
			float? delay = pendingCommand.delay;
			if (delay == null)
			{
				simulation.Router.Send(pendingCommand.c);
			}
			else
			{
				CommandRouter router = simulation.Router;
				Command c = pendingCommand.c;
				float? delay2 = pendingCommand.delay;
				router.Send(c, (ulong)delay2.Value);
			}
		}
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x0008FD60 File Offset: 0x0008DF60
	public float ComputeCircumscribedRadius()
	{
		float num = (this.Box.xmax - this.Box.xmin) / 2f;
		float num2 = (this.Box.ymax - this.Box.ymin) / 2f;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x0008FDB8 File Offset: 0x0008DFB8
	private Vector2 ComputeRandomOffsetFromTarget(Simulated target)
	{
		if (target.entity is ResidentEntity)
		{
			float num = this.ComputeCircumscribedRadius();
			float num2 = target.ComputeCircumscribedRadius();
			return UnityEngine.Random.insideUnitCircle.normalized * (num + num2);
		}
		return Vector2.zero;
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x0008FE00 File Offset: 0x0008E000
	public void TeleportUnitToTargetIfNeeded(Identity targetId, Simulation simulation)
	{
		Simulated simulated = simulation.FindSimulated(targetId);
		if (simulated != null)
		{
			Vector2 b = this.ComputeRandomOffsetFromTarget(simulated);
			Vector2 vector = simulated.PointOfInterest + b;
			this.Position = vector;
		}
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x0008FE38 File Offset: 0x0008E038
	public void EnableParticles(Simulation simulation, bool particlesEnabled)
	{
		if (particlesEnabled)
		{
			if (this.entity.Invariable.ContainsKey("fx.producing"))
			{
				ParticleSystemManager.Request request = (ParticleSystemManager.Request)this.entity.Invariable["fx.producing"];
				this.particlesRequest = simulation.particleSystemManager.RequestParticles(request.effectsName, request.initialPriority, request.subsequentPriority, request.cyclingPeriod, this.particleSystemRequestDelegate);
				if (this.particlesRequest != null && this.particleDisplayOffsetWorld == null)
				{
					this.particleDisplayOffsetWorld = new Vector3?(this.CameraOffsetToWorldVector(this.particleDisplayOffsetScreen, simulation.TheCamera));
				}
			}
		}
		else if (this.particlesRequest != null)
		{
			simulation.particleSystemManager.RemoveParticleSystemRequest(this.particlesRequest);
			this.particlesRequest = null;
		}
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x0008FF10 File Offset: 0x0008E110
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"[Simulated (ID=",
			this.Id,
			", name=",
			this.entity.Invariable["name"],
			", type=",
			this.entity.AllTypes,
			", DID=",
			this.entity.DefinitionId,
			", State=",
			this.action,
			")]"
		});
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x0008FFAC File Offset: 0x0008E1AC
	private Vector3 CameraOffsetToWorldVector(Vector3 offset, Camera camera)
	{
		double num = (double)offset.x * 0.1302;
		double num2 = (double)offset.y * 0.1302;
		Vector3 a = camera.transform.right;
		Vector3 vector = camera.transform.up;
		Vector3 b = offset.z * camera.transform.forward;
		a *= (float)num;
		vector *= (float)num2;
		return a + vector + b;
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x00090030 File Offset: 0x0008E230
	public void CalculateRushCompletionPercent(ulong endTime, ulong totalTime)
	{
		this.Variable[Simulated.RUSH_PERCENT] = (endTime - TFUtils.EpochTime()) / totalTime;
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x00090060 File Offset: 0x0008E260
	public void AddSimulateOnce(string key, Action action)
	{
		Dictionary<string, Action> dictionary;
		if (this.Variable.ContainsKey("simulate_once"))
		{
			dictionary = (Dictionary<string, Action>)this.Variable["simulate_once"];
		}
		else
		{
			dictionary = new Dictionary<string, Action>();
			this.Variable["simulate_once"] = dictionary;
		}
		dictionary[key] = action;
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x000900BC File Offset: 0x0008E2BC
	public void ClearSimulateOnce()
	{
		if (this.Variable.ContainsKey("simulate_once"))
		{
			this.Variable.Remove("simulate_once");
		}
	}

	// Token: 0x06001561 RID: 5473 RVA: 0x000900F0 File Offset: 0x0008E2F0
	public void SimulateOnce()
	{
		if (this.Variable.ContainsKey("simulate_once"))
		{
			foreach (Action action in ((Dictionary<string, Action>)this.Variable["simulate_once"]).Values)
			{
				action();
			}
			this.Variable.Remove("simulate_once");
		}
	}

	// Token: 0x06001562 RID: 5474 RVA: 0x00090190 File Offset: 0x0008E390
	public void RemoveSimulateOnceAction(string key)
	{
		if (this.Variable.ContainsKey("simulate_once"))
		{
			Dictionary<string, Action> dictionary = (Dictionary<string, Action>)this.Variable["simulate_once"];
			if (dictionary.ContainsKey(key))
			{
				dictionary.Remove(key);
			}
		}
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x000901DC File Offset: 0x0008E3DC
	public void DisableInteractions()
	{
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x000901E0 File Offset: 0x0008E3E0
	public void BillboardDelegate(Transform t, IDisplayController idc)
	{
		if (idc.isPerspectiveInArt)
		{
			SBCamera.BillboardDefinition(t, idc);
			return;
		}
		Vector3 vector = SBCamera.CameraUp();
		idc.BillboardScaling = Vector3.one;
		Vector3 rhs = new Vector3(-this.footprint.xmax, this.footprint.ymax, 0f);
		Vector3 vector2 = new Vector3(-1f, 1f, 0f);
		float x = 1f / Vector3.Dot(vector2.normalized, rhs.normalized);
		idc.BillboardScaling = new Vector3(x, 1f, 1f);
		t.LookAt(Vector3.Cross(vector, rhs), vector);
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x00090288 File Offset: 0x0008E488
	public void BlockerHighlight()
	{
		this.originalColor = this.displayController.Color;
		if (this.displayController.Transform.renderer != null && this.displayController.Transform.renderer.material.mainTexture.name.StartsWith("FishHouse") && this.displayController.Transform.renderer.material.shader == Shader.Find("Custom/TwoImageColorOverlay"))
		{
			this.displayController.Transform.renderer.material.shader = Shader.Find("Unlit/TransparentTint");
			this.displayController.Color *= this.BLOCKER_COLOR;
		}
		else
		{
			this.displayController.Color = this.BLOCKER_COLOR;
		}
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x00090374 File Offset: 0x0008E574
	public void ClearBlockerHighlight()
	{
		if (this.displayController.Transform.renderer != null && this.displayController.Transform.renderer.material.mainTexture.name.StartsWith("FishHouse"))
		{
			this.displayController.Transform.renderer.material.shader = Shader.Find("Custom/TwoImageColorOverlay");
		}
		this.displayController.Color = this.originalColor;
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x00090400 File Offset: 0x0008E600
	public void SetDisplayOffsetWorld(Simulation simulation)
	{
		this.thoughtDisplayOffsetWorld = new Vector3?(this.CameraOffsetToWorldVector(this.thoughtDisplayOffsetScreen, simulation.TheCamera));
	}

	// Token: 0x04000E96 RID: 3734
	private const int LOCK_WISH_DELAY = 60;

	// Token: 0x04000E97 RID: 3735
	public const bool DEBUG_LOG_STATEMACHINES = false;

	// Token: 0x04000E98 RID: 3736
	public const int TEMPTABLE_THRESHOLD = 0;

	// Token: 0x04000E99 RID: 3737
	private const string TIMEBAR_RUNNING = "timebar_running";

	// Token: 0x04000E9A RID: 3738
	public const string REQUEST_RUSH = "request_rush_sim";

	// Token: 0x04000E9B RID: 3739
	public const string IGNORE_REQUEST_RUSH = "ignore_request_rush_sim";

	// Token: 0x04000E9C RID: 3740
	private const string DC_EXT_NONE = "";

	// Token: 0x04000E9D RID: 3741
	private const string DC_EXT_FLIP = ".flip";

	// Token: 0x04000E9E RID: 3742
	private const string SIMULATE_ONCE = "simulate_once";

	// Token: 0x04000E9F RID: 3743
	public const string SHOW_TIMEBAR = "show_timebar";

	// Token: 0x04000EA0 RID: 3744
	public const string SHOW_NAMEBAR = "show_namebar";

	// Token: 0x04000EA1 RID: 3745
	public const string ENABLE_PARTICLES = "enable_particles";

	// Token: 0x04000EA2 RID: 3746
	public Reward taskBonusReward;

	// Token: 0x04000EA3 RID: 3747
	public Entity entity;

	// Token: 0x04000EA4 RID: 3748
	public int? forcedWish;

	// Token: 0x04000EA5 RID: 3749
	public bool showUnavailableIcon;

	// Token: 0x04000EA6 RID: 3750
	public Simulated.RushParameters rushParameters;

	// Token: 0x04000EA7 RID: 3751
	private string mStateModifierString;

	// Token: 0x04000EA8 RID: 3752
	public Simulated.SimulatedFlags simFlags;

	// Token: 0x04000EA9 RID: 3753
	private List<Action> clickListeners = new List<Action>();

	// Token: 0x04000EAA RID: 3754
	private static readonly List<Simulated.StateAction> prioritizedActions = new List<Simulated.StateAction>
	{
		EntityManager.BuildingActions["crafted"],
		EntityManager.BuildingActions["crafting"],
		EntityManager.BuildingActions["reflecting"],
		EntityManager.DebrisActions["deleting"],
		EntityManager.DebrisActions["clearing"],
		EntityManager.DebrisActions["clearing_more"],
		EntityManager.DebrisActions["priming_rush"],
		EntityManager.DebrisActions["inactive"]
	};

	// Token: 0x04000EAB RID: 3755
	private static readonly List<Simulated.StateAction> priorityOrder = new List<Simulated.StateAction>
	{
		EntityManager.ResidentActions["wait_bonus"],
		EntityManager.ResidentActions["task_collect_reward"],
		EntityManager.ResidentActions["wishing"],
		EntityManager.ResidentActions["task_wander"],
		EntityManager.ResidentActions["task_idle"],
		EntityManager.ResidentActions["task_stand"],
		EntityManager.ResidentActions["wander_full"],
		EntityManager.ResidentActions["idle_full"],
		EntityManager.ResidentActions["task_moving"],
		EntityManager.ResidentActions["tempted"],
		EntityManager.ResidentActions["not_tempted"]
	};

	// Token: 0x04000EAC RID: 3756
	public static readonly Color COLOR_FOOTPRINT_FREE = new Color(0.01f, 1f, 0.01f, 0.5f);

	// Token: 0x04000EAD RID: 3757
	public static readonly Color COLOR_FOOTPRINT_BLOCKED = new Color(1f, 0.01f, 0.01f, 0.5f);

	// Token: 0x04000EAE RID: 3758
	public static readonly Color COLOR_STANDARD = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04000EAF RID: 3759
	public static readonly Color COLOR_DRAGGING = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04000EB0 RID: 3760
	public static readonly string RUSH_PERCENT = "rush_percent";

	// Token: 0x04000EB1 RID: 3761
	public Simulated.ParticleSystemRequestDelegate particleSystemRequestDelegate;

	// Token: 0x04000EB2 RID: 3762
	public Simulated.ParticleSystemRequestDelegate rewardParticleSystemRequestDelegate;

	// Token: 0x04000EB3 RID: 3763
	public Simulated.ThoughtBubblePopParticleRequestDelegate thoughtBubblePopParticleRequestDelegate;

	// Token: 0x04000EB4 RID: 3764
	public Simulated.EatParticleRequestDelegate eatParticleRequestDelegate;

	// Token: 0x04000EB5 RID: 3765
	public Simulated.ActivateParticleRequestDelegate activateParticleSystemRequestDelegate;

	// Token: 0x04000EB6 RID: 3766
	public Simulated.SimulatedParticleRequestDelegate starsParticleSystemRequestDelegate;

	// Token: 0x04000EB7 RID: 3767
	public Simulated.SimulatedParticleRequestDelegate dustParticleSystemRequestDelegate;

	// Token: 0x04000EB8 RID: 3768
	public Simulated.TimebarMixinArgs timebarMixinArgs;

	// Token: 0x04000EB9 RID: 3769
	public Simulated.NamebarMixinArgs m_pNamebarMixinArgs;

	// Token: 0x04000EBA RID: 3770
	private Vector2[] position = new Vector2[]
	{
		default(Vector2),
		default(Vector2)
	};

	// Token: 0x04000EBB RID: 3771
	private Vector2 snapPosition = new Vector2(0f, 0f);

	// Token: 0x04000EBC RID: 3772
	private Vector2 pointOfInterestOffset = new Vector2(0f, 0f);

	// Token: 0x04000EBD RID: 3773
	private bool workerSpawner;

	// Token: 0x04000EBE RID: 3774
	private bool isWaypoint = true;

	// Token: 0x04000EBF RID: 3775
	private bool simulatedQueryable;

	// Token: 0x04000EC0 RID: 3776
	private AlignedBox footprint;

	// Token: 0x04000EC1 RID: 3777
	private AlignedBox box = new AlignedBox(-1f, 1f, -1f, 1f);

	// Token: 0x04000EC2 RID: 3778
	public AlignedBox snapBox = new AlignedBox(-1f, 1f, -1f, 1f);

	// Token: 0x04000EC3 RID: 3779
	public AlignedBox prevSceneBox = new AlignedBox(0f, 0f, 0f, 0f);

	// Token: 0x04000EC4 RID: 3780
	private Queue<Command> commands = new Queue<Command>();

	// Token: 0x04000EC5 RID: 3781
	private Command command;

	// Token: 0x04000EC6 RID: 3782
	protected TriggerableMixin triggerable = new TriggerableMixin();

	// Token: 0x04000EC7 RID: 3783
	private StateMachine<Simulated.StateAction, Command.TYPE> machine;

	// Token: 0x04000EC8 RID: 3784
	private Simulated.StateAction action;

	// Token: 0x04000EC9 RID: 3785
	private Dictionary<Simulated.StateAction, Queue<Command>> delegatedCommands = new Dictionary<Simulated.StateAction, Queue<Command>>();

	// Token: 0x04000ECA RID: 3786
	private bool visible;

	// Token: 0x04000ECB RID: 3787
	private Vector3 thoughtDisplayOffsetScreen = Vector3.zero;

	// Token: 0x04000ECC RID: 3788
	private Vector3? thoughtDisplayOffsetWorld;

	// Token: 0x04000ECD RID: 3789
	private Dictionary<string, Vector3> thoughtDisplayScreenOffsets;

	// Token: 0x04000ECE RID: 3790
	private Vector3 thoughtMaskDisplayOffsetScreen = Vector3.zero;

	// Token: 0x04000ECF RID: 3791
	private Vector3? thoughtMaskDisplayOffsetWorld;

	// Token: 0x04000ED0 RID: 3792
	private PeriodicPattern periodicMovement;

	// Token: 0x04000ED1 RID: 3793
	private PeriodicPattern thoughtItemBubbleScalingMajor;

	// Token: 0x04000ED2 RID: 3794
	private PeriodicPattern thoughtItemBubbleScalingMinor;

	// Token: 0x04000ED3 RID: 3795
	private IDisplayController thoughtDisplayController;

	// Token: 0x04000ED4 RID: 3796
	private IDisplayController thoughtMaskDisplayController;

	// Token: 0x04000ED5 RID: 3797
	private IDisplayController thoughtItemBubbleDisplayController;

	// Token: 0x04000ED6 RID: 3798
	private Vector3? thoughtItemBubbleDisplayOffsetWorld;

	// Token: 0x04000ED7 RID: 3799
	private Vector3 thoughtItemBubbleDisplayOffsetScreen = new Vector3(0f, -5f, 0.01f);

	// Token: 0x04000ED8 RID: 3800
	private SBGUIShadowedLabel thinkingLabel;

	// Token: 0x04000ED9 RID: 3801
	private SBGUIShadowedLabel thinkingSkipLabel;

	// Token: 0x04000EDA RID: 3802
	private SBGUIShadowedLabel thinkingSkipJjCounter;

	// Token: 0x04000EDB RID: 3803
	private SBGUIAtlasImage thinkingIcon;

	// Token: 0x04000EDC RID: 3804
	private SBGUIButton thinkingGhostButton;

	// Token: 0x04000EDD RID: 3805
	private Vector3 displayOffsetScreen = Vector3.zero;

	// Token: 0x04000EDE RID: 3806
	private Vector3? displayOffsetWorld;

	// Token: 0x04000EDF RID: 3807
	private Vector3 textureOriginScreen = Vector3.zero;

	// Token: 0x04000EE0 RID: 3808
	private Vector3? textureOriginWorld;

	// Token: 0x04000EE1 RID: 3809
	private IDisplayController displayController;

	// Token: 0x04000EE2 RID: 3810
	private string displayControllerExtension = string.Empty;

	// Token: 0x04000EE3 RID: 3811
	private bool displayControllerFlipped;

	// Token: 0x04000EE4 RID: 3812
	private IDisplayController footprintDisplayController;

	// Token: 0x04000EE5 RID: 3813
	private static IDisplayController footprintDisplayControllerShared;

	// Token: 0x04000EE6 RID: 3814
	private IDisplayController dropShadowDisplayController;

	// Token: 0x04000EE7 RID: 3815
	private InteractionState interactionState = new InteractionState();

	// Token: 0x04000EE8 RID: 3816
	private int selectionPriorityBaggage;

	// Token: 0x04000EE9 RID: 3817
	private List<Simulated.PendingCommand> pendingCommands = new List<Simulated.PendingCommand>();

	// Token: 0x04000EEA RID: 3818
	private ParticleSystemManager.Request particlesRequest;

	// Token: 0x04000EEB RID: 3819
	private Vector3 particleDisplayOffsetScreen = Vector3.zero;

	// Token: 0x04000EEC RID: 3820
	private Vector3? particleDisplayOffsetWorld;

	// Token: 0x04000EED RID: 3821
	private Scaffolding scaffolding;

	// Token: 0x04000EEE RID: 3822
	private Fence fence;

	// Token: 0x04000EEF RID: 3823
	private bool useFootprintIntersection;

	// Token: 0x04000EF0 RID: 3824
	private bool debugHitBoxesVisible;

	// Token: 0x04000EF1 RID: 3825
	private bool debugFootprintsVisible;

	// Token: 0x04000EF2 RID: 3826
	private IDisplayController debugQuadHitBoxDisplayController;

	// Token: 0x04000EF3 RID: 3827
	private IDisplayController debugThoughtBoxDisplayController;

	// Token: 0x04000EF4 RID: 3828
	private IDisplayController debugAlignedBoxDisplayController;

	// Token: 0x04000EF5 RID: 3829
	private string hitMeshName;

	// Token: 0x04000EF6 RID: 3830
	private bool separateTap;

	// Token: 0x04000EF7 RID: 3831
	private DateTime bounceStartTime;

	// Token: 0x04000EF8 RID: 3832
	private DateTime bounceStartStartTime;

	// Token: 0x04000EF9 RID: 3833
	private DateTime bounceEndStartTime;

	// Token: 0x04000EFA RID: 3834
	private bool calledBounceStart;

	// Token: 0x04000EFB RID: 3835
	private Color originalColor;

	// Token: 0x04000EFC RID: 3836
	private readonly Color BLOCKER_COLOR = new Color(1f, 0.55f, 0.45f);

	// Token: 0x04000EFD RID: 3837
	private bool swarmManaged;

	// Token: 0x020002AE RID: 686
	public class Annex
	{
		// Token: 0x0600156B RID: 5483 RVA: 0x00090494 File Offset: 0x0008E694
		public static Simulated Extend(Simulated simulated, Simulation simulation)
		{
			TFUtils.DebugLog("Extending as annex(name=" + (string)simulated.entity.Invariable["name"] + ")");
			simulated.entity = new AnnexEntity(simulated.entity);
			Simulated.Annex.SanityCheck(simulated, simulation);
			return simulated;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x000904E8 File Offset: 0x0008E6E8
		private static void SanityCheck(Simulated simulated, Simulation simulation)
		{
			AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
			Identity hubId = entity.HubId;
			TFUtils.Assert(entity.HubDid != null || simulation.FindSimulated(hubId) != null, string.Format("Could not find hub entity for this annex! Ensure it exists in starting state. \nExpectedHub={0}\nThisAnnex={1}", hubId, simulated));
		}

		// Token: 0x04000EFF RID: 3839
		public static Simulated.Annex.ActiveState Active = new Simulated.Annex.ActiveState();

		// Token: 0x04000F00 RID: 3840
		public static Simulated.Annex.RelayingState Relaying = new Simulated.Annex.RelayingState();

		// Token: 0x04000F01 RID: 3841
		public static Simulated.Annex.ShuntedCraftingState ShuntedCrafting = new Simulated.Annex.ShuntedCraftingState();

		// Token: 0x04000F02 RID: 3842
		public static Simulated.Annex.ShuntedCraftCyclingState ShuntedCraftCycling = new Simulated.Annex.ShuntedCraftCyclingState();

		// Token: 0x020002AF RID: 687
		public class ActiveState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x0600156E RID: 5486 RVA: 0x00090544 File Offset: 0x0008E744
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				Task task = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out task);
				if (task != null && task.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					task.m_sTargetPrevDisplayState = "idle";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("idle");
				}
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.SetInteractions(true, true, false, true, null, null);
				this.UpdateControls(simulation, simulated);
			}

			// Token: 0x0600156F RID: 5487 RVA: 0x000905DC File Offset: 0x0008E7DC
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x06001570 RID: 5488 RVA: 0x000905E0 File Offset: 0x0008E7E0
			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x06001571 RID: 5489 RVA: 0x000905E4 File Offset: 0x0008E7E4
			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = (!flag) ? "!!CANNOT_STOW_PRODUCTION" : null;
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					isEnabled = (flag = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null && text == null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null, true).Count > 0)
						{
							isEnabled = (flag = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable, null));
				if (simulation.featureManager.CheckFeature("move_reject_lock"))
				{
					list.Add(new Session.RejectControl());
				}
				simulated.InteractionState.PushControls(list);
			}
		}

		// Token: 0x020002B0 RID: 688
		public class RelayingState : Simulated.StateAction
		{
			// Token: 0x06001573 RID: 5491 RVA: 0x000907A8 File Offset: 0x0008E9A8
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.Clear();
				AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
				Identity identity = entity.HubId;
				if (identity == null && entity.HubDid != null)
				{
					Simulated simulated2 = simulation.FindSimulated(new int?((int)entity.HubDid.Value));
					if (simulated2 != null)
					{
						identity = simulated2.Id;
					}
				}
				if (identity == null)
				{
					TFUtils.DebugLog("Trying to relay to a hub that cannot be found!");
				}
				else
				{
					simulation.Router.Send(DelegateClickCommand.Create(simulated.Id, identity));
				}
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
			}

			// Token: 0x06001574 RID: 5492 RVA: 0x00090868 File Offset: 0x0008EA68
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x06001575 RID: 5493 RVA: 0x0009086C File Offset: 0x0008EA6C
			public void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		// Token: 0x020002B1 RID: 689
		public class ShuntedCraftingState : Simulated.Building.CraftingState
		{
			// Token: 0x06001577 RID: 5495 RVA: 0x00090878 File Offset: 0x0008EA78
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
				if (entity != null)
				{
					simulation.Router.Send(HUBCraftCommand.Create(simulated.Id, entity.HubId, true));
				}
				simulated.InteractionState.SetInteractions(true, true, false, true, null, null);
				this.UpdateControls(simulation, simulated);
			}

			// Token: 0x06001578 RID: 5496 RVA: 0x000908D8 File Offset: 0x0008EAD8
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
				if (entity != null)
				{
					simulation.Router.Send(HUBCraftCommand.Create(simulated.Id, entity.HubId, false));
				}
				base.Leave(simulation, simulated);
			}

			// Token: 0x06001579 RID: 5497 RVA: 0x00090918 File Offset: 0x0008EB18
			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = (!flag) ? "!!CANNOT_STOW_PRODUCTION" : null;
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					isEnabled = (flag = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null && text == null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null, true).Count > 0)
						{
							isEnabled = (flag = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable, null));
				if (simulation.featureManager.CheckFeature("move_reject_lock"))
				{
					list.Add(new Session.RejectControl());
				}
				simulated.InteractionState.PushControls(list);
			}

			// Token: 0x0600157A RID: 5498 RVA: 0x00090AD4 File Offset: 0x0008ECD4
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return base.Simulate(simulation, simulated, session);
			}
		}

		// Token: 0x020002B2 RID: 690
		public class ShuntedCraftCyclingState : Simulated.Building.CraftCyclingState
		{
			// Token: 0x0600157C RID: 5500 RVA: 0x00090AF4 File Offset: 0x0008ECF4
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
				if (entity != null)
				{
					simulation.Router.Send(HUBCraftCommand.Create(simulated.Id, entity.HubId, true));
				}
				simulated.InteractionState.SetInteractions(true, true, false, true, null, null);
			}

			// Token: 0x0600157D RID: 5501 RVA: 0x00090B4C File Offset: 0x0008ED4C
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
				if (entity != null)
				{
					simulation.Router.Send(HUBCraftCommand.Create(simulated.Id, entity.HubId, false));
				}
				base.Leave(simulation, simulated);
			}
		}
	}

	// Token: 0x020002B3 RID: 691
	public class Building
	{
		// Token: 0x06001580 RID: 5504 RVA: 0x00090C88 File Offset: 0x0008EE88
		public static Simulated Load(BuildingEntity buildingEntity, Simulation simulation, Vector2 position, bool flip, ulong utcNow)
		{
			if (flip && !buildingEntity.Flippable)
			{
				flip = false;
			}
			ErectableDecorator decorator = buildingEntity.GetDecorator<ErectableDecorator>();
			ActivatableDecorator decorator2 = buildingEntity.GetDecorator<ActivatableDecorator>();
			string text;
			if (decorator2.Activated == 0UL)
			{
				text = "prime_erecting";
			}
			else
			{
				text = "reflecting";
			}
			if (SBSettings.ConsoleLoggingEnabled)
			{
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"Loading building(name=",
					(string)buildingEntity.Invariable["name"],
					", id=",
					buildingEntity.Id,
					", did=",
					buildingEntity.DefinitionId,
					", state=",
					text
				}), TFUtils.LogFilter.Buildings);
			}
			Simulated simulated = simulation.CreateSimulated(buildingEntity, EntityManager.BuildingActions[text], position);
			simulated.Warp(position, simulation);
			simulated.Visible = true;
			simulated.Flip = flip;
			simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
			if (text == "prime_erecting" && !simulated.Variable.ContainsKey("employee"))
			{
				Simulated simulated2 = simulation.SpawnWorker(simulated);
				ulong delay = decorator.ErectionTime;
				if (decorator.ErectionCompleteTime != null)
				{
					delay = decorator.ErectionCompleteTime.Value - utcNow;
				}
				simulation.Router.Send(EmployCommand.Create(Identity.Null(), simulated.Id, simulated2.Id));
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated2.Id), delay);
				simulation.Router.Send(ReturnCommand.Create(simulated.Id, simulated2.Id), delay);
				simulation.Router.Send(ErectCommand.Create(simulated2.Id, simulated2.Id, Identity.Null(), 0UL));
				simulated.Variable["employee"] = simulated2.Id;
				Simulated.Building.AdjustWorkerPosition(simulated, simulation);
			}
			else if (text == "prime_erecting")
			{
				Simulated.Building.AdjustWorkerPosition(simulated, simulation);
			}
			simulated.SetFootprint(simulation, true);
			return simulated;
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00090EA4 File Offset: 0x0008F0A4
		public static void AdjustWorkerPosition(Simulated building, Simulation simulation)
		{
			if (!building.Variable.ContainsKey("employee"))
			{
				return;
			}
			Identity identity = building.Variable["employee"] as Identity;
			if (identity != null)
			{
				Simulated simulated = simulation.FindSimulated(identity);
				if (simulated != null && simulated.Position != building.PointOfInterest)
				{
					simulated.Warp(building.PointOfInterest, simulation);
				}
			}
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00090F14 File Offset: 0x0008F114
		public static Simulated TryAddResident(Simulation simulation, Simulated building, int? residentDid, Identity existingResidentInstance = null)
		{
			if (residentDid != null)
			{
				ulong num = TFUtils.EpochTime();
				Simulated simulated;
				if (existingResidentInstance == null)
				{
					ResidentEntity residentEntity = simulation.EntityManager.Create(EntityType.RESIDENT, residentDid.Value, null, true).GetDecorator<ResidentEntity>();
					if (residentEntity.Disabled)
					{
						return null;
					}
					residentEntity.HungerResourceId = null;
					residentEntity.PreviousResourceId = null;
					residentEntity.WishExpiresAt = null;
					residentEntity.HungryAt = num;
					residentEntity.MatchBonus = null;
					simulated = Simulated.Resident.Load(residentEntity, building.Id, residentEntity.WishExpiresAt, residentEntity.HungerResourceId, residentEntity.PreviousResourceId, residentEntity.HungryAt, null, residentEntity.MatchBonus, simulation, num);
				}
				else
				{
					ResidentEntity residentEntity = (ResidentEntity)simulation.EntityManager.GetEntity(existingResidentInstance);
					if (residentEntity.Disabled)
					{
						return null;
					}
					simulated = Simulated.Resident.Load(residentEntity, building.Id, residentEntity.WishExpiresAt, residentEntity.HungerResourceId, residentEntity.PreviousResourceId, residentEntity.HungryAt, new ulong?(residentEntity.FullnessLength), residentEntity.MatchBonus, simulation, num);
				}
				if (simulated != null)
				{
					simulated.Warp(building.PointOfInterest, simulation);
					simulated.Visible = true;
				}
				return simulated;
			}
			return null;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00091050 File Offset: 0x0008F250
		public static List<Simulated> FindResidents(Simulation simulation, Simulated building)
		{
			List<Simulated> list = new List<Simulated>();
			BuildingEntity entity = building.GetEntity<BuildingEntity>();
			if (entity.HasResident)
			{
				foreach (Simulated simulated in simulation.GetSimulateds())
				{
					if (simulated.entity is ResidentEntity && simulated.GetEntity<ResidentEntity>().Residence.Equals(building.Id))
					{
						list.Add(simulated);
					}
				}
			}
			return list;
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x000910F8 File Offset: 0x0008F2F8
		public static void AddResidentToGameState(Dictionary<string, object> gameState, string residentId, int residentDid, string residenceId, ulong residentHungryTime)
		{
			List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["units"];
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did"] = residentDid;
			dictionary["label"] = residentId;
			dictionary["residence"] = residenceId;
			dictionary["feed_ready_time"] = residentHungryTime;
			dictionary["waiting"] = false;
			dictionary["active"] = true;
			list.Add(dictionary);
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x00091190 File Offset: 0x0008F390
		public static void RemoveResidentsFromGameState(Dictionary<string, object> gameState, string buildingId)
		{
			List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["units"];
			Predicate<object> match = (object u) => ((string)((Dictionary<string, object>)u)["residence"]).Equals(buildingId);
			list.RemoveAll(match);
		}

		// Token: 0x04000F03 RID: 3843
		public const string WORKER = "employee";

		// Token: 0x04000F04 RID: 3844
		public static Simulated.Building.PlacingAction Placing = new Simulated.Building.PlacingAction();

		// Token: 0x04000F05 RID: 3845
		public static Simulated.Building.PrimeErectingState PrimeErecting = new Simulated.Building.PrimeErectingState();

		// Token: 0x04000F06 RID: 3846
		public static Simulated.Building.PrimeErectingStateFriend PrimeErectingFriend = new Simulated.Building.PrimeErectingStateFriend();

		// Token: 0x04000F07 RID: 3847
		public static Simulated.Building.ErectingState Erecting = new Simulated.Building.ErectingState();

		// Token: 0x04000F08 RID: 3848
		public static Simulated.Building.InactiveState Inactive = new Simulated.Building.InactiveState();

		// Token: 0x04000F09 RID: 3849
		public static Simulated.Building.ActiveState Active = new Simulated.Building.ActiveState();

		// Token: 0x04000F0A RID: 3850
		public static Simulated.Building.RequestingInterfaceState RequestingInterface = new Simulated.Building.RequestingInterfaceState();

		// Token: 0x04000F0B RID: 3851
		public static Simulated.Building.ReflectingState Reflecting = new Simulated.Building.ReflectingState();

		// Token: 0x04000F0C RID: 3852
		public static Simulated.Building.ReplacingState Replacing = new Simulated.Building.ReplacingState();

		// Token: 0x04000F0D RID: 3853
		public static Simulated.Building.ActivatingState Activating = new Simulated.Building.ActivatingState();

		// Token: 0x04000F0E RID: 3854
		public static Simulated.Building.ReactivatingState Reactivating = new Simulated.Building.ReactivatingState();

		// Token: 0x04000F0F RID: 3855
		public static Simulated.Building.ProducingState Producing = new Simulated.Building.ProducingState();

		// Token: 0x04000F10 RID: 3856
		public static Simulated.Building.ProducedState Produced = new Simulated.Building.ProducedState();

		// Token: 0x04000F11 RID: 3857
		public static Simulated.Building.CraftingState Crafting = new Simulated.Building.CraftingState();

		// Token: 0x04000F12 RID: 3858
		public static Simulated.Building.CraftedState Crafted = new Simulated.Building.CraftedState();

		// Token: 0x04000F13 RID: 3859
		public static Simulated.Building.CraftCyclingState CraftCycling = new Simulated.Building.CraftCyclingState();

		// Token: 0x04000F14 RID: 3860
		public static Simulated.Building.CraftingCollectState CraftingCollect = new Simulated.Building.CraftingCollectState();

		// Token: 0x04000F15 RID: 3861
		public static Simulated.Building.RushingBuildState RushingBuild = new Simulated.Building.RushingBuildState();

		// Token: 0x04000F16 RID: 3862
		public static Simulated.Building.RushingProductState RushingProduct = new Simulated.Building.RushingProductState();

		// Token: 0x04000F17 RID: 3863
		public static Simulated.Building.RushingCraftState RushingCraft = new Simulated.Building.RushingCraftState();

		// Token: 0x04000F18 RID: 3864
		public static Simulated.Building.FriendsParkInactiveState FriendParkInactive = new Simulated.Building.FriendsParkInactiveState();

		// Token: 0x04000F19 RID: 3865
		public static Simulated.Building.TaskFeedState TaskFeed = new Simulated.Building.TaskFeedState();

		// Token: 0x04000F1A RID: 3866
		public static Simulated.Building.TaskFeedCollectingState TaskFeedCollecting = new Simulated.Building.TaskFeedCollectingState();

		// Token: 0x020002B4 RID: 692
		public class PlacingAction : Simulated.StateActionBuildingDefault
		{
			// Token: 0x06001587 RID: 5511 RVA: 0x000911E8 File Offset: 0x0008F3E8
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				Simulated.Building.PlacingAction.Setup(simulated, simulation);
			}

			// Token: 0x06001588 RID: 5512 RVA: 0x00091200 File Offset: 0x0008F400
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				ErectableDecorator entity2 = simulated.GetEntity<ErectableDecorator>();
				if (entity2.ErectionTime > 0UL)
				{
					Identity identity = simulated.command["employee"] as Identity;
					simulated.Variable["employee"] = identity;
					simulation.Router.Send(MoveCommand.Create(simulated.Id, identity, simulated.PointOfInterest, simulated.Flip));
				}
				entity2.ErectionCompleteTime = new ulong?(TFUtils.EpochTime() + entity2.ErectionTime);
				entity.Slots = simulation.game.craftManager.GetInitialSlots(entity.DefinitionId);
				this.RecordPlacement(simulation, simulated);
			}

			// Token: 0x06001589 RID: 5513 RVA: 0x000912B0 File Offset: 0x0008F4B0
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x0600158A RID: 5514 RVA: 0x000912B4 File Offset: 0x0008F4B4
			public static void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
				simulated.InteractionState.Clear();
				BaseTransitionBinding transition = null;
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				if (simulation.featureManager.CheckFeature("move_reject_lock"))
				{
					list.Add(new Session.StashControl(simulated, false, "!!CANNOT_STOW_PLACEMENT"));
					list.Add(new Session.SellControl(simulated, false, "!!CANNOT_SELL_PLACEMENT"));
				}
				else
				{
					list.Add(new Session.StashControl(simulated, false, string.Empty));
					list.Add(new Session.SellControl(simulated, false, string.Empty));
				}
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable, null));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, true, false, transition, list);
			}

			// Token: 0x0600158B RID: 5515 RVA: 0x00091398 File Offset: 0x0008F598
			private void RecordPlacement(Simulation simulation, Simulated simulated)
			{
				Cost cost = simulation.catalog.GetCost(simulated.entity.DefinitionId);
				simulation.ModifyGameStateSimulated(simulated, new NewBuildingAction(simulated, cost));
			}
		}

		// Token: 0x020002B5 RID: 693
		public class PrimeErectingState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x0600158D RID: 5517 RVA: 0x000913D8 File Offset: 0x0008F5D8
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				ErectableDecorator erectable = simulated.GetEntity<ErectableDecorator>();
				if (erectable.ErectionTime > 0UL)
				{
					simulated.timebarMixinArgs.hasTimebar = true;
					simulated.timebarMixinArgs.description = Language.Get((string)simulated.Entity.Invariable["name"]);
					simulated.timebarMixinArgs.completeTime = erectable.ErectionCompleteTime.Value;
					simulated.timebarMixinArgs.totalTime = erectable.ErectionTime;
					simulated.timebarMixinArgs.duration = erectable.ErectionTimerDuration;
					simulated.timebarMixinArgs.rushCost = erectable.BuildRushCost;
					simulated.m_pNamebarMixinArgs.m_bHasNamebar = false;
					ulong started = erectable.ErectionCompleteTime.Value - erectable.ErectionTime;
					Action<Session> execute = delegate(Session session)
					{
						this.Rush(session, simulated);
						int nJellyCost = 0;
						Cost.Prorate(erectable.BuildRushCost, started, erectable.ErectionCompleteTime.Value, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out nJellyCost);
						string sItemName = "Accelerate_" + erectable.Name;
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, erectable.DefinitionId, nJellyCost, sItemName, "buildings", "speedup", "construction", "confirm");
					};
					Action<Session> cancel = delegate(Session session)
					{
						int num = 0;
						Cost.Prorate(erectable.BuildRushCost, started, erectable.ErectionCompleteTime.Value, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out num);
					};
					simulated.rushParameters = new Simulated.RushParameters(execute, cancel, (ulong time) => Cost.Prorate(erectable.BuildRushCost, started, erectable.ErectionCompleteTime.Value, time), Language.Get("!!RUSH_BUILD") + " " + simulated.Entity.BlueprintName, simulated.Entity.DefinitionId, delegate(Session session, Cost cost, bool canAfford)
					{
						this.LogRush(session, simulated, cost, canAfford);
					}, simulation.ScreenPositionFromWorldPosition(simulated.DisplayController.Position));
				}
			}

			// Token: 0x0600158E RID: 5518 RVA: 0x000915BC File Offset: 0x0008F7BC
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				session.TheGame.selected = simulated;
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				ulong? erectionCompleteTime = entity.ErectionCompleteTime;
				if (erectionCompleteTime != null && erectionCompleteTime.Value > TFUtils.EpochTime())
				{
					new Session.TimebarGroup().ActivateOnSelected(session);
				}
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				session.TheGame.selected = null;
				return false;
			}

			// Token: 0x0600158F RID: 5519 RVA: 0x0009163C File Offset: 0x0008F83C
			private void Rush(Session session, Simulated simulated)
			{
				session.TheGame.simulation.Router.Send(RushCommand.Create(simulated.Id));
			}

			// Token: 0x06001590 RID: 5520 RVA: 0x0009166C File Offset: 0x0008F86C
			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
				session.analytics.LogRushBuild(simulated.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
			}
		}

		// Token: 0x020002B6 RID: 694
		public class PrimeErectingStateFriend : Simulated.StateActionBuildingDefault
		{
			// Token: 0x06001592 RID: 5522 RVA: 0x000916AC File Offset: 0x0008F8AC
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x06001593 RID: 5523 RVA: 0x000916B0 File Offset: 0x0008F8B0
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x06001594 RID: 5524 RVA: 0x000916B4 File Offset: 0x0008F8B4
			private void Rush(Session session, Simulated simulated)
			{
			}

			// Token: 0x06001595 RID: 5525 RVA: 0x000916B8 File Offset: 0x0008F8B8
			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
			}
		}

		// Token: 0x020002B7 RID: 695
		public class ErectingState : Simulated.StateActionBuildingDefault, Simulated.Animated
		{
			// Token: 0x06001597 RID: 5527 RVA: 0x000916C4 File Offset: 0x0008F8C4
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.AddScaffolding(simulation);
				simulated.AddFence(simulation);
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				BaseTransitionBinding transition = null;
				BuildingEntity entity2 = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				list.Add(new Session.StashControl(simulated, false, "!!CANNOT_STOW_CONSTRUCTING"));
				list.Add(new Session.SellControl(simulated, false, "!!CANNOT_SELL_CONSTRUCTING"));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity2.Flippable, simulation));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, true, false, transition, list);
				simulated.useFootprintIntersection = true;
				if (entity.ErectionCompleteTime == null)
				{
					entity.ErectionCompleteTime = new ulong?(TFUtils.EpochTime() + entity.ErectionTime);
				}
				ulong num = entity.ErectionCompleteTime.Value - TFUtils.EpochTime();
				if (num > 0UL)
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), num);
					entity.RaisingTimeRemaining = num;
				}
				else
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				}
				simulated.EnableAnimateAction(true);
			}

			// Token: 0x06001598 RID: 5528 RVA: 0x00091804 File Offset: 0x0008FA04
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.useFootprintIntersection = false;
				simulated.ClearSimulateOnce();
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.m_pNamebarMixinArgs.m_bHasNamebar = false;
				simulated.Visible = true;
				if (simulated.displayController != null && simulated.displayController.Transform.GetComponent<MeshRenderer>() != null && !simulated.displayController.Transform.renderer.material.HasProperty("_AlphaTex"))
				{
					simulated.Color = new Color(1f, 1f, 1f, 1f);
				}
				simulated.RemoveScaffolding(simulation);
				simulated.RemoveFence(simulation);
				simulated.GetEntity<ErectableDecorator>().RaisingTimeRemaining = 0.0;
				if (simulated.DisplayController is BasicSprite)
				{
					BasicSprite basicSprite = (BasicSprite)simulated.DisplayController;
					basicSprite.SetMaskPercentage(0f);
				}
				simulation.particleSystemManager.RemoveRequestWithDelegate(simulated.dustParticleSystemRequestDelegate);
				simulated.dustParticleSystemRequestDelegate.isAssigned = false;
				simulation.particleSystemManager.RemoveRequestWithDelegate(simulated.starsParticleSystemRequestDelegate);
				simulated.starsParticleSystemRequestDelegate.isAssigned = false;
				object obj = null;
				if (simulated.Variable.TryGetValue("employee", out obj))
				{
					Identity identity = obj as Identity;
					if (identity != null)
					{
						simulation.Router.Send(ReturnCommand.Create(simulated.Id, identity));
					}
				}
				simulated.EnableAnimateAction(false);
			}

			// Token: 0x06001599 RID: 5529 RVA: 0x00091970 File Offset: 0x0008FB70
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.SimulateOnce();
				return false;
			}

			// Token: 0x0600159A RID: 5530 RVA: 0x0009197C File Offset: 0x0008FB7C
			public Vector3 Animate(Simulation simulation, Simulated simulated)
			{
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				if (entity.ErectionTime <= 0UL)
				{
					return Vector3.zero;
				}
				float num = (float)(entity.RaisingTimeRemaining / entity.ErectionTime);
				num = TFMath.ClampF(num, 0f, 1f);
				entity.RaisingTimeRemaining -= (double)Time.deltaTime;
				float d;
				if (num > 0.5f)
				{
					if (simulated.scaffolding != null)
					{
						simulated.scaffolding.SetHeight(simulation.enclosureManager, (1f - num) / 0.5f * simulated.Height * 0.33f, new BillboardDelegate(SBCamera.BillboardDefinition));
					}
					d = -simulated.Height;
					simulated.DisplayController.SetMaskPercentage(1f);
				}
				else
				{
					if (!simulated.dustParticleSystemRequestDelegate.isAssigned)
					{
						simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Construction_Smoke", 0, 0, 1f, simulated.dustParticleSystemRequestDelegate);
						simulated.dustParticleSystemRequestDelegate.isAssigned = true;
					}
					if (!simulated.starsParticleSystemRequestDelegate.isAssigned)
					{
						simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Construction_Stars", 0, 0, 1f, simulated.starsParticleSystemRequestDelegate);
						simulated.starsParticleSystemRequestDelegate.isAssigned = true;
					}
					float num2 = num / 0.5f;
					d = -num2 * simulated.Height;
					float num3 = (simulated.Height <= 10f) ? 0.15f : (20f / simulated.Height);
					simulated.DisplayController.SetMaskPercentage(num2 + num3);
				}
				return simulated.DisplayController.Up * d;
			}

			// Token: 0x04000F1B RID: 3867
			public const string CLICK_DURATION_HANDLER = "clickDurationHandler";
		}

		// Token: 0x020002B8 RID: 696
		public class InactiveState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x0600159C RID: 5532 RVA: 0x00091B28 File Offset: 0x0008FD28
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				if (entity.ErectionTime == 0UL)
				{
					simulation.Router.Send(ClickedCommand.Create(simulated.Id, simulated.Id));
				}
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				BaseTransitionBinding transition = null;
				BuildingEntity entity2 = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.StashControl(simulated, false, "!!CANNOT_STOW_CONSTRUCTING"));
				list.Add(new Session.SellControl(simulated, false, "!!CANNOT_SELL_CONSTRUCTING"));
				list.Add(new Session.RotateControl(simulated, entity2.Flippable, null));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, true, true, transition, list);
				simulated.DisplayState("default");
				simulated.DisplayThoughtState(simulated.GetEntity<ErectableDecorator>().CompletionReward.Summary.ThoughtIcon, "default", simulation);
				simulation.game.triggerRouter.RouteTrigger(SimulatedTrigger.CreateTrigger(simulated, "contruction_complete"), simulation.game);
				if (entity.ErectionTime != 0UL)
				{
					simulated.DisplayController.SetMaskPercentage(0f);
				}
			}

			// Token: 0x0600159D RID: 5533 RVA: 0x00091C48 File Offset: 0x0008FE48
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity.Invariable.ContainsKey("completion_sound"))
				{
					simulation.soundEffectManager.PlaySound((string)entity.Invariable["completion_sound"]);
				}
				else
				{
					simulation.soundEffectManager.PlaySound("UnveilBuilding");
				}
				simulated.DisplayState("default");
			}

			// Token: 0x0600159E RID: 5534 RVA: 0x00091CB4 File Offset: 0x0008FEB4
			public new bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		// Token: 0x020002B9 RID: 697
		public class ActiveState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015A0 RID: 5536 RVA: 0x00091CC0 File Offset: 0x0008FEC0
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				if (simulated.command != null && simulated.command.Type == Command.TYPE.HUBCRAFT)
				{
					BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
					if (entity == null)
					{
						TFUtils.ErrorLog("Building(" + simulated.Id.Describe() + "):Active Enter called with null building entity");
					}
					bool flag = (bool)simulated.command["start"];
					if (flag)
					{
						if (entity.BusyAnnexCount == 0)
						{
							simulated.AddSimulateOnce("enable_particles", delegate
							{
								simulated.EnableParticles(simulation, true);
							});
						}
						entity.BusyAnnexCount++;
					}
					else
					{
						if (entity.BusyAnnexCount == 1)
						{
							simulated.ClearSimulateOnce();
							simulated.EnableParticles(simulation, false);
						}
						if (entity.BusyAnnexCount > 0)
						{
							entity.BusyAnnexCount--;
						}
					}
				}
				List<int> activeSourcesWithMatchBonusForTarget = simulation.game.taskManager.GetActiveSourcesWithMatchBonusForTarget(simulation, simulated.Id);
				if (activeSourcesWithMatchBonusForTarget.Count > 0)
				{
					simulation.Router.Send(BonusRewardCommand.Create(simulated.Id, simulated.Id, activeSourcesWithMatchBonusForTarget[0]));
					simulated.command = null;
					return;
				}
				this.TryProduce(simulation, simulated.GetEntity<BuildingEntity>());
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				Simulated.Building.ActiveState.Setup(simulated, simulation);
				this.UpdateControls(simulation, simulated);
			}

			// Token: 0x060015A1 RID: 5537 RVA: 0x00091EA0 File Offset: 0x000900A0
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity == null)
				{
					TFUtils.ErrorLog("Building(" + simulated.Id.Describe() + "):Active Leave called with null building entity");
				}
				if (entity.BusyAnnexCount == 0)
				{
					simulated.ClearSimulateOnce();
					simulated.EnableParticles(simulation, false);
				}
			}

			// Token: 0x060015A2 RID: 5538 RVA: 0x00091EF4 File Offset: 0x000900F4
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity == null)
				{
					TFUtils.ErrorLog("Building(" + simulated.Id.Describe() + "):Simulate called with null building entity");
				}
				if (entity.BusyAnnexCount > 0)
				{
					simulated.SimulateOnce();
				}
				return false;
			}

			// Token: 0x060015A3 RID: 5539 RVA: 0x00091F40 File Offset: 0x00090140
			public static void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.InteractionState.SetInteractions(true, true, false, false, null, null);
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity.CanCraft)
				{
					simulated.InteractionState.SelectedTransition = new Session.BrowseRecipesTransition(simulated);
				}
				else if (entity.CanVend)
				{
					simulated.InteractionState.SelectedTransition = new Session.VendingTransition(simulated);
				}
				else
				{
					simulated.InteractionState.IsSelectable = true;
					simulated.m_pNamebarMixinArgs.m_bHasNamebar = true;
					simulated.m_pNamebarMixinArgs.m_sName = Language.Get(entity.Name);
					simulated.m_pNamebarMixinArgs.m_bCheckForTaskCharacters = true;
				}
				Task task = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out task);
				if (task != null && task.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					task.m_sTargetPrevDisplayState = "default";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("default");
				}
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}

			// Token: 0x060015A4 RID: 5540 RVA: 0x00092050 File Offset: 0x00090250
			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = (!flag) ? "!!CANNOT_STOW_PRODUCTION" : null;
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					isEnabled = (flag = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null, true).Count > 0)
						{
							isEnabled = (flag = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable, null));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.PushControls(list);
			}

			// Token: 0x060015A5 RID: 5541 RVA: 0x000921F0 File Offset: 0x000903F0
			public void TryProduce(Simulation simulation, BuildingEntity building)
			{
				if (building.HasDecorator<PeriodicProductionDecorator>())
				{
					simulation.Router.Send(ProduceCommand.Create(building.Id, building.Id));
				}
			}
		}

		// Token: 0x020002BA RID: 698
		public class RequestingInterfaceState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015A7 RID: 5543 RVA: 0x0009222C File Offset: 0x0009042C
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
			}

			// Token: 0x060015A8 RID: 5544 RVA: 0x00092238 File Offset: 0x00090438
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				session.AddAsyncResponse("RequestEntityInterface", simulated, false);
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				return false;
			}

			// Token: 0x060015A9 RID: 5545 RVA: 0x00092270 File Offset: 0x00090470
			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		// Token: 0x020002BB RID: 699
		public class ReflectingState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015AB RID: 5547 RVA: 0x0009227C File Offset: 0x0009047C
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
			}

			// Token: 0x060015AC RID: 5548 RVA: 0x00092288 File Offset: 0x00090488
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<int> activeSourcesWithMatchBonusForTarget = simulation.game.taskManager.GetActiveSourcesWithMatchBonusForTarget(simulation, simulated.Id);
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				Command command;
				if (activeSourcesWithMatchBonusForTarget.Count > 0)
				{
					command = BonusRewardCommand.Create(simulated.Id, simulated.Id, activeSourcesWithMatchBonusForTarget[0]);
				}
				else if (entity.HasDecorator<PeriodicProductionDecorator>())
				{
					command = ProduceCommand.Create(simulated.Id, simulated.Id);
				}
				else if (entity.CraftRewards != null)
				{
					command = AdvanceCommand.Create(simulated.Id, simulated.Id);
				}
				else if (simulation.game.craftManager.Crafting(entity.Id))
				{
					command = CraftCommand.Create(simulated.Id, simulated.Id);
				}
				else
				{
					command = ActivateCommand.Create(simulated.Id, simulated.Id);
				}
				TFUtils.Assert(command != null, "This building couldn't figure out what to do!");
				simulation.Router.Send(command);
				return false;
			}

			// Token: 0x060015AD RID: 5549 RVA: 0x00092388 File Offset: 0x00090588
			public new void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		// Token: 0x020002BC RID: 700
		public class ReplacingState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015AF RID: 5551 RVA: 0x00092394 File Offset: 0x00090594
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				this.Setup(simulated, simulation);
			}

			// Token: 0x060015B0 RID: 5552 RVA: 0x000923AC File Offset: 0x000905AC
			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x060015B1 RID: 5553 RVA: 0x000923B0 File Offset: 0x000905B0
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x060015B2 RID: 5554 RVA: 0x000923B4 File Offset: 0x000905B4
			private void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
				simulated.InteractionState.Clear();
				this.UpdateControls(simulation, simulated);
			}

			// Token: 0x060015B3 RID: 5555 RVA: 0x000923F4 File Offset: 0x000905F4
			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BaseTransitionBinding transition = null;
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null, true).Count > 0)
						{
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable, null));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, true, false, transition, list);
			}
		}

		// Token: 0x020002BD RID: 701
		public abstract class ActivatingBase : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015B5 RID: 5557 RVA: 0x0009254C File Offset: 0x0009074C
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				List<Simulated> residents = this.GetResidents(simulation, simulated);
				this.UpdateBuildingState(simulated);
				this.RecordActions(simulation, simulated, residents);
				simulated.command = null;
				simulated.InteractionState.Clear();
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
			}

			// Token: 0x060015B6 RID: 5558
			protected abstract List<Simulated> GetResidents(Simulation simulation, Simulated building);

			// Token: 0x060015B7 RID: 5559
			protected abstract void UpdateBuildingState(Simulated simulated);

			// Token: 0x060015B8 RID: 5560
			protected abstract void RecordActions(Simulation simulation, Simulated simulated, List<Simulated> residents);

			// Token: 0x060015B9 RID: 5561 RVA: 0x000925C0 File Offset: 0x000907C0
			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x060015BA RID: 5562 RVA: 0x000925C4 File Offset: 0x000907C4
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		// Token: 0x020002BE RID: 702
		public class ActivatingState : Simulated.Building.ActivatingBase
		{
			// Token: 0x060015BC RID: 5564 RVA: 0x000925D0 File Offset: 0x000907D0
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
			}

			// Token: 0x060015BD RID: 5565 RVA: 0x000925DC File Offset: 0x000907DC
			protected override List<Simulated> GetResidents(Simulation simulation, Simulated building)
			{
				List<Simulated> list = new List<Simulated>();
				List<int> residentDids = building.GetEntity<BuildingEntity>().ResidentDids;
				foreach (int num in residentDids)
				{
					Simulated simulated = null;
					if (num != -1)
					{
						simulated = Simulated.Building.TryAddResident(simulation, building, new int?(num), null);
					}
					if (simulated != null)
					{
						list.Add(simulated);
					}
				}
				return list;
			}

			// Token: 0x060015BE RID: 5566 RVA: 0x00092674 File Offset: 0x00090874
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Building_Pop", 0, 0, 0f, simulated.activateParticleSystemRequestDelegate);
			}

			// Token: 0x060015BF RID: 5567 RVA: 0x00092694 File Offset: 0x00090894
			protected override void UpdateBuildingState(Simulated simulated)
			{
				ulong num = TFUtils.EpochTime();
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
				decorator.Activated = num;
				if (entity.HasDecorator<PeriodicProductionDecorator>())
				{
					PeriodicProductionDecorator decorator2 = entity.GetDecorator<PeriodicProductionDecorator>();
					decorator2.ProductReadyTime = num + decorator2.RentProductionTime;
				}
			}

			// Token: 0x060015C0 RID: 5568 RVA: 0x000926DC File Offset: 0x000908DC
			protected override void RecordActions(Simulation simulation, Simulated simulated, List<Simulated> residents)
			{
				Reward reward = null;
				if (simulated.GetEntity<ErectableDecorator>().CompletionReward != null)
				{
					reward = simulated.GetEntity<ErectableDecorator>().CompletionReward.GenerateReward(simulation, false);
				}
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow, false);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Building.ActivatingState.RecordActions - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
				CompleteBuildingAction completeBuildingAction = new CompleteBuildingAction(simulated, residents, reward);
				completeBuildingAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, completeBuildingAction);
				completeBuildingAction.AddPickup(simulation);
				simulated.entity.PatchReferences(simulation.game);
				simulation.soundEffectManager.PlaySound("PopResourceBubble");
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Pop", 0, 0, 0f, simulated.rewardParticleSystemRequestDelegate);
			}

			// Token: 0x060015C1 RID: 5569 RVA: 0x000927C4 File Offset: 0x000909C4
			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				return entity.CompletionReward.GenerateReward(simulation, false);
			}
		}

		// Token: 0x020002BF RID: 703
		public class ReactivatingState : Simulated.Building.ActivatingBase
		{
			// Token: 0x060015C3 RID: 5571 RVA: 0x000927F0 File Offset: 0x000909F0
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity.Invariable.ContainsKey("completion_sound"))
				{
					simulation.soundEffectManager.PlaySound((string)entity.Invariable["completion_sound"]);
				}
				else
				{
					simulation.soundEffectManager.PlaySound("UnveilBuilding");
				}
				simulated.SimulatedQueryable = true;
				base.Enter(simulation, simulated);
			}

			// Token: 0x060015C4 RID: 5572 RVA: 0x00092860 File Offset: 0x00090A60
			protected override List<Simulated> GetResidents(Simulation simulation, Simulated building)
			{
				List<KeyValuePair<int, Identity>> list = (List<KeyValuePair<int, Identity>>)building.Variable["associated_entities"];
				if (list != null && list.Count > 0)
				{
					List<Simulated> list2 = new List<Simulated>();
					foreach (KeyValuePair<int, Identity> keyValuePair in list)
					{
						Simulated simulated = Simulated.Building.TryAddResident(simulation, building, new int?(keyValuePair.Key), keyValuePair.Value);
						if (simulated != null)
						{
							ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
							ulong num = entity.HungryAt;
							if (num < 0UL)
							{
								num = 0UL;
							}
							num += TFUtils.EpochTime();
							entity.HungryAt = num;
							list2.Add(simulated);
						}
					}
					return list2;
				}
				List<int> residentDids = building.GetEntity<BuildingEntity>().ResidentDids;
				int count = residentDids.Count;
				if (count <= 0)
				{
					return null;
				}
				List<Simulated> list3 = new List<Simulated>();
				for (int i = 0; i < count; i++)
				{
					Simulated simulated2 = Simulated.Building.TryAddResident(simulation, building, new int?(residentDids[i]), null);
					if (simulated2 != null)
					{
						list3.Add(simulated2);
					}
				}
				return list3;
			}

			// Token: 0x060015C5 RID: 5573 RVA: 0x000929AC File Offset: 0x00090BAC
			protected override void UpdateBuildingState(Simulated simulated)
			{
				ulong num = TFUtils.EpochTime();
				if (simulated.HasEntity<PeriodicProductionDecorator>())
				{
					PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
					entity.ProductReadyTime = num + entity.RentProductionTime;
				}
			}

			// Token: 0x060015C6 RID: 5574 RVA: 0x000929E0 File Offset: 0x00090BE0
			protected override void RecordActions(Simulation simulation, Simulated simulated, List<Simulated> residents)
			{
				simulation.ModifyGameStateSimulated(simulated, new MoveAction(simulated, residents));
			}
		}

		// Token: 0x020002C0 RID: 704
		public class ProducingState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015C8 RID: 5576 RVA: 0x00092A00 File Offset: 0x00090C00
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				List<int> activeSourcesWithMatchBonusForTarget = simulation.game.taskManager.GetActiveSourcesWithMatchBonusForTarget(simulation, simulated.Id);
				if (activeSourcesWithMatchBonusForTarget.Count > 0)
				{
					simulation.Router.Send(BonusRewardCommand.Create(simulated.Id, simulated.Id, activeSourcesWithMatchBonusForTarget[0]));
					return;
				}
				PeriodicProductionDecorator productProducer = simulated.GetEntity<PeriodicProductionDecorator>();
				ulong num = TFUtils.EpochTime();
				ulong num2 = (num < productProducer.ProductReadyTime) ? (productProducer.ProductReadyTime - num) : 0UL;
				if (SBSettings.ConsoleLoggingEnabled)
				{
					TFUtils.DebugLog(string.Concat(new object[]
					{
						"Building(",
						simulated.Id.Describe(),
						"):Producing. Ready in ",
						num2,
						" at ",
						productProducer.ProductReadyTime
					}), TFUtils.LogFilter.Buildings);
				}
				simulated.SimulatedQueryable = true;
				Task task = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out task);
				if (task != null && task.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					task.m_sTargetPrevDisplayState = "producing";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("producing");
				}
				simulated.DisplayThoughtState(null, simulation);
				BuildingEntity building = simulated.GetEntity<BuildingEntity>();
				this.UpdateControls(simulation, simulated);
				string str = Language.Get("!!TASKBAR_PRODUCING_RENT");
				if (productProducer.Product.Summary != null && !productProducer.Product.Summary.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
				{
					foreach (int num3 in productProducer.Product.Summary.ResourceAmounts.Keys)
					{
						if (num3 != ResourceManager.XP)
						{
							str = string.Format(Language.Get("!!TASKBAR_PRODUCING"), Language.Get(simulation.resourceManager.Resources[num3].Name));
							break;
						}
					}
				}
				if (productProducer.RentRushable)
				{
					simulated.timebarMixinArgs.hasTimebar = true;
					simulated.timebarMixinArgs.description = Language.Get((string)simulated.Entity.Invariable["name"]) + "|" + str;
					simulated.timebarMixinArgs.completeTime = productProducer.ProductReadyTime;
					simulated.timebarMixinArgs.totalTime = productProducer.RentProductionTime;
					simulated.timebarMixinArgs.duration = productProducer.RentTimerDuration;
					simulated.timebarMixinArgs.rushCost = productProducer.RentRushCost;
					simulated.timebarMixinArgs.m_bCheckForTaskCharacters = true;
					simulated.m_pNamebarMixinArgs.m_bHasNamebar = false;
				}
				else
				{
					simulated.m_pNamebarMixinArgs.m_bHasNamebar = true;
					simulated.m_pNamebarMixinArgs.m_sName = Language.Get(building.Name);
					simulated.m_pNamebarMixinArgs.m_bCheckForTaskCharacters = true;
					simulated.timebarMixinArgs.hasTimebar = false;
				}
				ulong started = productProducer.ProductReadyTime - productProducer.RentProductionTime;
				Action<Session> execute = delegate(Session session)
				{
					this.Rush(session, simulated);
					int nJellyCost = 0;
					Cost.Prorate(productProducer.RentRushCost, started, productProducer.ProductReadyTime, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out nJellyCost);
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, building.DefinitionId, nJellyCost, "Accelerate_rent_collected_" + building.Name, "buildings", "speedup", "rent", "confirm");
				};
				Action<Session> cancel = delegate(Session session)
				{
					int num4 = 0;
					Cost.Prorate(productProducer.RentRushCost, started, productProducer.ProductReadyTime, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out num4);
				};
				simulated.rushParameters = new Simulated.RushParameters(execute, cancel, (ulong time) => Cost.Prorate(productProducer.RentRushCost, started, productProducer.ProductReadyTime, time), Language.Get("!!RUSH_RENT") + " " + productProducer.BlueprintName, productProducer.DefinitionId, delegate(Session session, Cost cost, bool canAfford)
				{
					this.LogRush(session, simulated, cost, canAfford);
				}, simulation.ScreenPositionFromWorldPosition(simulated.DisplayController.Position));
				simulated.AddSimulateOnce("enable_particles", delegate
				{
					simulated.EnableParticles(simulation, true);
				});
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), num2);
			}

			// Token: 0x060015C9 RID: 5577 RVA: 0x00092F5C File Offset: 0x0009115C
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.ClearSimulateOnce();
				simulated.EnableParticles(simulation, false);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.m_pNamebarMixinArgs.m_bHasNamebar = false;
			}

			// Token: 0x060015CA RID: 5578 RVA: 0x00092F90 File Offset: 0x00091190
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.SimulateOnce();
				return false;
			}

			// Token: 0x060015CB RID: 5579 RVA: 0x00092F9C File Offset: 0x0009119C
			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BaseTransitionBinding transition = null;
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = (!flag) ? "!!CANNOT_STOW_PRODUCTION" : null;
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					isEnabled = (flag = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null, true).Count > 0)
						{
							isEnabled = (flag = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable, null));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, true, false, transition, list);
			}

			// Token: 0x060015CC RID: 5580 RVA: 0x0009314C File Offset: 0x0009134C
			private void Rush(Session session, Simulated simulated)
			{
				session.TheGame.simulation.Router.Send(RushCommand.Create(simulated.Id));
			}

			// Token: 0x060015CD RID: 5581 RVA: 0x0009317C File Offset: 0x0009137C
			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
				session.analytics.LogRushRent(simulated.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
			}
		}

		// Token: 0x020002C1 RID: 705
		public class ProducedState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015CF RID: 5583 RVA: 0x000931BC File Offset: 0x000913BC
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				Simulated.Building.ProducedState.Setup(simulation, simulated);
				this.UpdateControls(simulation, simulated);
			}

			// Token: 0x060015D0 RID: 5584 RVA: 0x000931DC File Offset: 0x000913DC
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
				ulong num = TFUtils.EpochTime();
				entity.ProductReadyTime = entity.RentProductionTime + num;
				this.SpawnDrops(simulation, simulated);
			}

			// Token: 0x060015D1 RID: 5585 RVA: 0x0009320C File Offset: 0x0009140C
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x060015D2 RID: 5586 RVA: 0x00093210 File Offset: 0x00091410
			public static void Setup(Simulation simulation, Simulated simulated)
			{
				Task task = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out task);
				if (task != null && task.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					task.m_sTargetPrevDisplayState = "produced";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("produced");
				}
				simulated.DisplayThoughtState("produced", simulation);
				simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
			}

			// Token: 0x060015D3 RID: 5587 RVA: 0x00093290 File Offset: 0x00091490
			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = (!flag) ? "!!CANNOT_STOW_PRODUCTION" : null;
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					isEnabled = (flag = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null, true).Count > 0)
						{
							isEnabled = (flag = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable, null));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, false, true, null, list);
			}

			// Token: 0x060015D4 RID: 5588 RVA: 0x00093434 File Offset: 0x00091634
			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
				Reward reward = this.GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow, false);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Building.ProducedState.SpawnDrops - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
				CollectRentAction collectRentAction;
				if (SBSettings.UseActionFile)
				{
					collectRentAction = new CollectRentAction(simulated, reward);
				}
				else
				{
					PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
					collectRentAction = new CollectRentAction(simulated, reward, entity.ProductReadyTime);
				}
				AnalyticsWrapper.LogRentCollected(simulation.game, simulated, reward);
				collectRentAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, collectRentAction);
				collectRentAction.AddPickup(simulation);
				simulation.analytics.LogCollectRentReward(simulated.entity.DefinitionId, simulation.resourceManager.PlayerLevelAmount);
				simulation.soundEffectManager.PlaySound("PopResourceBubble");
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Pop", 0, 0, 0f, simulated.rewardParticleSystemRequestDelegate);
			}

			// Token: 0x060015D5 RID: 5589 RVA: 0x00093544 File Offset: 0x00091744
			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
				return entity.Product.GenerateReward(simulation, true, false);
			}
		}

		// Token: 0x020002C2 RID: 706
		public class CraftingState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015D7 RID: 5591 RVA: 0x00093570 File Offset: 0x00091770
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				List<int> activeSourcesWithMatchBonusForTarget = simulation.game.taskManager.GetActiveSourcesWithMatchBonusForTarget(simulation, simulated.Id);
				if (activeSourcesWithMatchBonusForTarget.Count > 0)
				{
					simulation.Router.Send(BonusRewardCommand.Create(simulated.Id, simulated.Id, activeSourcesWithMatchBonusForTarget[0]));
					return;
				}
				simulated.InteractionState.SetInteractions(true, true, true, false, null, null);
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity.CanCraft)
				{
					simulated.InteractionState.SelectedTransition = new Session.BrowseRecipesTransition(simulated);
				}
				Task task = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out task);
				if (task != null && task.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					task.m_sTargetPrevDisplayState = "crafting";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("crafting");
				}
				simulated.DisplayThoughtState(null, simulation);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.m_pNamebarMixinArgs.m_bHasNamebar = false;
				simulated.AddSimulateOnce("enable_particles", delegate
				{
					simulated.EnableParticles(simulation, true);
				});
			}

			// Token: 0x060015D8 RID: 5592 RVA: 0x00093730 File Offset: 0x00091930
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.ClearSimulateOnce();
				simulated.EnableParticles(simulation, false);
			}

			// Token: 0x060015D9 RID: 5593 RVA: 0x00093740 File Offset: 0x00091940
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.SimulateOnce();
				return false;
			}
		}

		// Token: 0x020002C3 RID: 707
		public class CraftedState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015DB RID: 5595 RVA: 0x00093754 File Offset: 0x00091954
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				Simulated.Building.CraftedState.Setup(simulation, simulated);
				this.UpdateControls(simulation, simulated);
			}

			// Token: 0x060015DC RID: 5596 RVA: 0x00093774 File Offset: 0x00091974
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				if (!simulation.craftManager.Crafting(simulated.Id))
				{
					BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
					Reward reward = this.GetReward(simulation, simulated);
					ulong utcNow = TFUtils.EpochTime();
					RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow, false);
					if (rewardDropResults == null)
					{
						TFUtils.ErrorLog("Building.CraftedState.Leave - dropResults is null");
						return;
					}
					int count = rewardDropResults.dropIdentities.Count;
					Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
					CraftCollectAction craftCollectAction = new CraftCollectAction(simulated.Id, reward);
					craftCollectAction.AddDropData(simulated, dropID);
					simulation.ModifyGameStateSimulated(simulated, craftCollectAction);
					craftCollectAction.AddPickup(simulation);
					simulation.analytics.LogCollectCraftedGood(simulated.entity.DefinitionId, simulation.resourceManager.PlayerLevelAmount);
					Dictionary<int, int> resourceAmounts = reward.ResourceAmounts;
					foreach (KeyValuePair<int, int> keyValuePair in resourceAmounts)
					{
						if (keyValuePair.Value > 0 && simulation.craftManager.ContainsRecipe(keyValuePair.Key))
						{
							CraftingRecipe recipeById = simulation.craftManager.GetRecipeById(keyValuePair.Key);
							if (recipeById != null)
							{
								AnalyticsWrapper.LogCraftCollected(simulation.game, entity, keyValuePair.Key, keyValuePair.Value, recipeById.recipeName);
							}
						}
					}
					simulation.soundEffectManager.PlaySound("PopResourceBubble");
					simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Pop", 0, 0, 0f, simulated.rewardParticleSystemRequestDelegate);
					entity.ClearCraftingRewards();
				}
			}

			// Token: 0x060015DD RID: 5597 RVA: 0x00093934 File Offset: 0x00091B34
			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = (!flag) ? "!!CANNOT_STOW_PRODUCTION" : null;
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					isEnabled = (flag = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null, true).Count > 0)
						{
							isEnabled = (flag = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable, null));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, false, true, null, list);
			}

			// Token: 0x060015DE RID: 5598 RVA: 0x00093AD8 File Offset: 0x00091CD8
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x060015DF RID: 5599 RVA: 0x00093ADC File Offset: 0x00091CDC
			public static void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.InteractionState.SetInteractions(true, false, false, true, null, null);
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				string text;
				if (entity.OverrideRewardTexture != null)
				{
					text = entity.OverrideRewardTexture;
				}
				else if (entity.Slots > 1)
				{
					TFUtils.ErrorLog("For buildings with multiple production slots, specify an override reward texture!");
					text = simulation.resourceManager.Resources[ResourceManager.XP].GetResourceTexture();
				}
				else if (entity.CraftRewards == null)
				{
					TFUtils.ErrorLog("Buildings that reach the crafted state should have CraftRewards set. How did you get here?");
					text = simulation.resourceManager.Resources[ResourceManager.XP].GetResourceTexture();
				}
				else
				{
					text = entity.CraftRewards.ThoughtIcon;
				}
				Task task = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out task);
				if (task != null && task.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					task.m_sTargetPrevDisplayState = "produced";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("produced");
				}
				if (text != null)
				{
					simulated.DisplayThoughtState(text, "produced", simulation);
				}
				else
				{
					simulated.DisplayThoughtState("produced", simulation);
				}
			}

			// Token: 0x060015E0 RID: 5600 RVA: 0x00093C0C File Offset: 0x00091E0C
			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x060015E1 RID: 5601 RVA: 0x00093C10 File Offset: 0x00091E10
			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				return entity.CraftRewards;
			}
		}

		// Token: 0x020002C4 RID: 708
		public class CraftCyclingState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015E3 RID: 5603 RVA: 0x00093C34 File Offset: 0x00091E34
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (simulated.command != null && simulated.command.Type == Command.TYPE.CRAFTED)
				{
					TFUtils.Assert(simulated.command.HasProperty("slot_id"), "Missing SlotID property from Crafted Command");
					if (simulated.command.HasProperty("slot_id"))
					{
						int num = (int)simulated.command["slot_id"];
						CraftingInstance craftingInstance = simulation.craftManager.GetCraftingInstance(simulated.Id, num);
						if (craftingInstance != null)
						{
							simulation.craftManager.RemoveCraftingInstance(simulated.Id, num);
							entity.CraftingComplete(craftingInstance.reward);
							CraftingRecipe recipeById = simulation.craftManager.GetRecipeById(craftingInstance.recipeId);
							simulation.soundEffectManager.PlaySound(recipeById.readySoundImmediate);
							simulation.soundEffectManager.PlaySound(recipeById.readySoundBeat, recipeById.beatLength);
							if (craftingInstance.recipeId == 1000)
							{
								simulation.soundEffectManager.PlaySound("lettuce_ready");
							}
							else if (craftingInstance.recipeId == 1003)
							{
								simulation.soundEffectManager.PlaySound("tomato_ready");
							}
							CraftCompleteAction action = new CraftCompleteAction(simulated.Id, num, craftingInstance.reward);
							simulation.ModifyGameStateSimulated(simulated, action);
						}
					}
				}
				simulated.command = null;
				simulated.InteractionState.SetInteractions(true, true, true, true, null, null);
				Task task = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out task);
				if (task != null && task.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					task.m_sTargetPrevDisplayState = "crafting";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("crafting");
				}
				if (entity.CraftRewards != null)
				{
					if (entity.CraftRewards.ThoughtIcon != null)
					{
						simulated.DisplayThoughtState(entity.CraftRewards.ThoughtIcon, "produced", simulation);
					}
					else
					{
						simulated.DisplayThoughtState("produced", simulation);
					}
				}
				if (!simulation.craftManager.Crafting(simulated.Id))
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				}
			}
		}

		// Token: 0x020002C5 RID: 709
		public class CraftingCollectState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015E5 RID: 5605 RVA: 0x00093E7C File Offset: 0x0009207C
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.InteractionState.SetInteractions(false, false, false, false, null, null);
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
			}

			// Token: 0x060015E6 RID: 5606 RVA: 0x00093EC0 File Offset: 0x000920C0
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				this.SpawnDrops(simulation, simulated);
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				entity.ClearCraftingRewards();
			}

			// Token: 0x060015E7 RID: 5607 RVA: 0x00093EE4 File Offset: 0x000920E4
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x060015E8 RID: 5608 RVA: 0x00093EE8 File Offset: 0x000920E8
			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
				Reward reward = this.GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow, false);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Building.CraftingCollectState.SpawnDrops - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
				CraftCollectAction craftCollectAction = new CraftCollectAction(simulated.Id, reward);
				craftCollectAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, craftCollectAction);
				craftCollectAction.AddPickup(simulation);
				simulation.soundEffectManager.PlaySound("PopResourceBubble");
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity != null)
				{
					Dictionary<int, int> resourceAmounts = reward.ResourceAmounts;
					foreach (KeyValuePair<int, int> keyValuePair in resourceAmounts)
					{
						if (keyValuePair.Value > 0 && simulation.craftManager.ContainsRecipe(keyValuePair.Key))
						{
							CraftingRecipe recipeById = simulation.craftManager.GetRecipeById(keyValuePair.Key);
							if (recipeById != null)
							{
								AnalyticsWrapper.LogCraftCollected(simulation.game, entity, keyValuePair.Key, keyValuePair.Value, recipeById.recipeName);
							}
						}
					}
				}
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Pop", 0, 0, 0f, simulated.rewardParticleSystemRequestDelegate);
			}

			// Token: 0x060015E9 RID: 5609 RVA: 0x00094070 File Offset: 0x00092270
			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				return entity.CraftRewards;
			}
		}

		// Token: 0x020002C6 RID: 710
		public class RushingBuildState : Simulated.RushingSomething
		{
			// Token: 0x060015EB RID: 5611 RVA: 0x00094094 File Offset: 0x00092294
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				simulated.CalculateRushCompletionPercent(entity.ErectionCompleteTime.Value, entity.ErectionTime);
				entity.ErectionCompleteTime = new ulong?(TFUtils.EpochTime());
				base.Enter(simulation, simulated);
			}

			// Token: 0x060015EC RID: 5612 RVA: 0x000940DC File Offset: 0x000922DC
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				Identity identity = (!simulated.Variable.ContainsKey("employee")) ? null : (simulated.Variable["employee"] as Identity);
				if (identity != null)
				{
					simulation.FindSimulated(identity).ClearPendingCommands();
					simulation.Router.Send(ReturnCommand.Create(simulated.Id, identity));
				}
				simulation.game.notificationManager.CancelNotification("build:" + simulated.Id.Describe());
				Cost cost = new Cost();
				cost += simulated.GetEntity<ErectableDecorator>().BuildRushCost;
				cost.Prorate((float)simulated.Variable[Simulated.RUSH_PERCENT]);
				simulation.ModifyGameStateSimulated(simulated, new RushBuildAction(simulated.Id, cost, simulated.GetEntity<ErectableDecorator>().ErectionCompleteTime.Value));
			}

			// Token: 0x060015ED RID: 5613 RVA: 0x000941C8 File Offset: 0x000923C8
			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<ErectableDecorator>().BuildRushCost;
			}
		}

		// Token: 0x020002C7 RID: 711
		public class RushingProductState : Simulated.RushingSomething
		{
			// Token: 0x060015EF RID: 5615 RVA: 0x000941E0 File Offset: 0x000923E0
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
				simulated.CalculateRushCompletionPercent(entity.ProductReadyTime, entity.RentProductionTime);
				entity.ProductReadyTime = TFUtils.EpochTime();
				base.Enter(simulation, simulated);
			}

			// Token: 0x060015F0 RID: 5616 RVA: 0x0009421C File Offset: 0x0009241C
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				Cost cost = new Cost();
				PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
				cost += entity.RentRushCost;
				cost.Prorate((float)simulated.Variable[Simulated.RUSH_PERCENT]);
				simulation.ModifyGameStateSimulated(simulated, new RushRentAction(simulated.Id, cost, entity.ProductReadyTime));
			}

			// Token: 0x060015F1 RID: 5617 RVA: 0x0009427C File Offset: 0x0009247C
			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<PeriodicProductionDecorator>().RentRushCost;
			}
		}

		// Token: 0x020002C8 RID: 712
		public class RushingCraftState : Simulated.RushingSomething
		{
			// Token: 0x060015F3 RID: 5619 RVA: 0x00094294 File Offset: 0x00092494
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				if (simulated.command == null)
				{
					TFUtils.ErrorLog("RushingCraftState:Enter - command is null");
					return;
				}
				TFUtils.Assert(simulated.command.HasProperty("slot_id"), "Trying to rush building " + simulated.Id + " without supplying a slot_id");
				int num = (int)simulated.command["slot_id"];
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"Building(",
					simulated.Id.Describe(),
					"):RushingCraft on slot ",
					num
				}));
				CraftingInstance craftingInstance = simulation.craftManager.GetCraftingInstance(simulated.Id, num);
				if (craftingInstance == null)
				{
					simulated.command = null;
					TFUtils.ErrorLog("RushingCraftState:Enter - instance is null");
					return;
				}
				CraftingRecipe recipeById = simulation.craftManager.GetRecipeById(craftingInstance.recipeId);
				if (recipeById == null)
				{
					simulated.command = null;
					TFUtils.ErrorLog("RushingCraftState:Enter - recipe is null");
					return;
				}
				int num2 = simulation.Router.CancelMatching(Command.TYPE.CRAFTED, simulated.Id, simulated.Id, new Dictionary<string, object>
				{
					{
						"slot_id",
						num
					}
				});
				TFUtils.Assert(num2 == 1, string.Concat(new object[]
				{
					"Expected rush to only cancel exactly 1 command in the command router. But is instead replacing ",
					num2,
					" commands targetting ",
					num
				}));
				simulated.CalculateRushCompletionPercent(craftingInstance.ReadyTimeUtc, recipeById.craftTime);
				Cost cost = new Cost();
				cost += recipeById.rushCost;
				cost.Prorate((float)simulated.Variable[Simulated.RUSH_PERCENT]);
				simulation.ModifyGameStateSimulated(simulated, new RushCraftAction(simulated.Id, num, cost, craftingInstance.ReadyTimeUtc, craftingInstance.reward));
				simulation.Router.Send(CraftedCommand.Create(simulated.Id, simulated.Id, num));
				ResourceManager resourceManager = simulation.resourceManager;
				if (resourceManager.CanPay(cost))
				{
					resourceManager.Apply(new Cost(cost), simulation.game);
				}
				else
				{
					TFUtils.Assert(false, "You don't have enough money! Consider showing an insufficient funds dialog before getting here!");
				}
				simulated.command = null;
			}

			// Token: 0x060015F4 RID: 5620 RVA: 0x000944B8 File Offset: 0x000926B8
			public override void CancelCurrentCommands(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x060015F5 RID: 5621 RVA: 0x000944BC File Offset: 0x000926BC
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.game.notificationManager.CancelNotification("craft:" + simulated.Id.Describe());
			}

			// Token: 0x060015F6 RID: 5622 RVA: 0x000944E4 File Offset: 0x000926E4
			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				throw new NotImplementedException("Get Rush Cost is currently not supported for Craft Buildings");
			}
		}

		// Token: 0x020002C9 RID: 713
		public class FriendsParkInactiveState : Simulated.StateAction
		{
			// Token: 0x060015F8 RID: 5624 RVA: 0x000944F8 File Offset: 0x000926F8
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				simulated.InteractionState.SetInteractions(false, false, false, false, null, null);
				simulated.simFlags |= Simulated.SimulatedFlags.FORCE_ANIMATE_ACTION;
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}

			// Token: 0x060015F9 RID: 5625 RVA: 0x00094548 File Offset: 0x00092748
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x060015FA RID: 5626 RVA: 0x0009454C File Offset: 0x0009274C
			public void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		// Token: 0x020002CA RID: 714
		public class TaskFeedState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x060015FC RID: 5628 RVA: 0x00094558 File Offset: 0x00092758
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<BuildingEntity>().TaskSourceFeedDID = (int)simulated.command["source_id"];
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulation.soundEffectManager.PlaySound("MatchBonus_Ready");
				Simulated.Building.TaskFeedState.Setup(simulation, simulated);
				this.UpdateControls(simulation, simulated);
			}

			// Token: 0x060015FD RID: 5629 RVA: 0x000945B4 File Offset: 0x000927B4
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.soundEffectManager.PlaySound("MatchBonus_Open");
				this.CollectAndRecordRewards(simulation, simulated);
			}

			// Token: 0x060015FE RID: 5630 RVA: 0x000945D0 File Offset: 0x000927D0
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x060015FF RID: 5631 RVA: 0x000945D4 File Offset: 0x000927D4
			public static void Setup(Simulation simulation, Simulated simulated)
			{
				Task task = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out task);
				if (task != null && task.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					task.m_sTargetPrevDisplayState = "default";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("default");
				}
				simulated.DisplayThoughtState("bonus_ready", simulation);
				simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
			}

			// Token: 0x06001600 RID: 5632 RVA: 0x00094654 File Offset: 0x00092854
			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = (!flag) ? "!!CANNOT_STOW_PRODUCTION" : null;
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					isEnabled = (flag = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null, true).Count > 0)
						{
							isEnabled = (flag = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable, null));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, false, true, null, list);
			}

			// Token: 0x06001601 RID: 5633 RVA: 0x000947F8 File Offset: 0x000929F8
			private void CollectAndRecordRewards(Simulation simulation, Simulated simulated)
			{
				Simulated simulated2 = simulation.FindSimulated(new int?(simulated.GetEntity<BuildingEntity>().TaskSourceFeedDID));
				ResidentEntity entity = simulated2.GetEntity<ResidentEntity>();
				Reward matchBonus = entity.MatchBonus;
				entity.MatchBonus = null;
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(matchBonus, simulation, simulated, TFUtils.EpochTime(), false);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Building.TaskFeedState.CollectAndRecordRewards - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
				CollectMatchBonusAction collectMatchBonusAction = new CollectMatchBonusAction(simulated2.Id, matchBonus);
				collectMatchBonusAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated2, collectMatchBonusAction);
				collectMatchBonusAction.AddPickup(simulation);
				AnalyticsWrapper.LogBonusChest(simulation.game, simulated2, matchBonus);
				if (ResourceManager.SPONGY_GAMES_CURRENCY >= 0)
				{
					int b = 0;
					if (matchBonus.ResourceAmounts.TryGetValue(ResourceManager.SPONGY_GAMES_CURRENCY, out b))
					{
						SBMISoaring.AddFoodToCharacter(b, simulated.GetEntity<ResidentEntity>().DefinitionId, -1, null);
					}
				}
			}
		}

		// Token: 0x020002CB RID: 715
		public class TaskFeedCollectingState : Simulated.StateActionBuildingDefault
		{
			// Token: 0x06001603 RID: 5635 RVA: 0x00094904 File Offset: 0x00092B04
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Task task = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out task);
				if (task != null && task.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					task.m_sTargetPrevDisplayState = "default";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("default");
				}
				simulated.DisplayThoughtState("bonus_collect", simulation);
				simulated.InteractionState.SetInteractions(false, false, false, false, null, null);
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), 1UL);
			}

			// Token: 0x06001604 RID: 5636 RVA: 0x000949AC File Offset: 0x00092BAC
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x06001605 RID: 5637 RVA: 0x000949B0 File Offset: 0x00092BB0
			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		// Token: 0x020004AD RID: 1197
		// (Invoke) Token: 0x06002517 RID: 9495
		public delegate void Setup(Simulation simulation, Simulated simulated);
	}

	// Token: 0x020002CC RID: 716
	public class Debris
	{
		// Token: 0x06001608 RID: 5640 RVA: 0x00094A1C File Offset: 0x00092C1C
		public static Simulated Load(DebrisEntity debrisEntity, Simulation simulation, Vector2 position, ulong utcNow)
		{
			ClearableDecorator decorator = debrisEntity.GetDecorator<ClearableDecorator>();
			PurchasableDecorator decorator2 = debrisEntity.GetDecorator<PurchasableDecorator>();
			string key;
			if (!decorator2.Purchased)
			{
				key = "unpurchased";
			}
			else
			{
				if (decorator.ClearCompleteTime != null)
				{
					ulong? clearCompleteTime = decorator.ClearCompleteTime;
					if (clearCompleteTime != null && clearCompleteTime.Value <= utcNow)
					{
						key = "deleting";
						goto IL_B7;
					}
				}
				if (decorator.ClearCompleteTime != null)
				{
					ulong? clearCompleteTime2 = decorator.ClearCompleteTime;
					if (clearCompleteTime2 != null && clearCompleteTime2.Value > utcNow)
					{
						key = "clearing";
						goto IL_B7;
					}
				}
				key = "inactive";
			}
			IL_B7:
			Simulated simulated = simulation.CreateSimulated(debrisEntity, EntityManager.DebrisActions[key], position);
			simulated.Warp(position, simulation);
			simulated.Visible = true;
			simulated.SetFootprint(simulation, true);
			return simulated;
		}

		// Token: 0x04000F1C RID: 3868
		public static Simulated.Debris.UnpurchasedState Unpurchased = new Simulated.Debris.UnpurchasedState();

		// Token: 0x04000F1D RID: 3869
		public static Simulated.Debris.InactiveState Inactive = new Simulated.Debris.InactiveState();

		// Token: 0x04000F1E RID: 3870
		public static Simulated.Debris.ClearingState Clearing = new Simulated.Debris.ClearingState();

		// Token: 0x04000F1F RID: 3871
		public static Simulated.Debris.ClearingMoreInfoState ClearingMoreInfo = new Simulated.Debris.ClearingMoreInfoState();

		// Token: 0x04000F20 RID: 3872
		public static Simulated.Debris.PrimingRushState PrimingRush = new Simulated.Debris.PrimingRushState();

		// Token: 0x04000F21 RID: 3873
		public static Simulated.Debris.RushingClearingState RushingClearing = new Simulated.Debris.RushingClearingState();

		// Token: 0x04000F22 RID: 3874
		public static Simulated.Debris.DeletingState Deleting = new Simulated.Debris.DeletingState();

		// Token: 0x04000F23 RID: 3875
		public static Simulated.Debris.DeletedState Deleted = new Simulated.Debris.DeletedState();

		// Token: 0x020002CD RID: 717
		public class UnpurchasedState : Simulated.StateAction
		{
			// Token: 0x0600160A RID: 5642 RVA: 0x00094B14 File Offset: 0x00092D14
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Simulated.Debris.UnpurchasedState.Setup(simulation, simulated);
			}

			// Token: 0x0600160B RID: 5643 RVA: 0x00094B24 File Offset: 0x00092D24
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.Variable["purchased"] = true;
				simulated.DisplayState("default");
			}

			// Token: 0x0600160C RID: 5644 RVA: 0x00094B48 File Offset: 0x00092D48
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.InteractionState.IsSelectable = Session.TheDebugManager.debugPlaceObjects;
				return false;
			}

			// Token: 0x0600160D RID: 5645 RVA: 0x00094B60 File Offset: 0x00092D60
			public static void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.InteractionState.Clear();
				simulated.DisplayState("inactive");
			}
		}

		// Token: 0x020002CE RID: 718
		public class InactiveState : Simulated.StateAction
		{
			// Token: 0x0600160F RID: 5647 RVA: 0x00094B80 File Offset: 0x00092D80
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Simulated.Debris.InactiveState.Setup(simulation, simulated);
			}

			// Token: 0x06001610 RID: 5648 RVA: 0x00094B90 File Offset: 0x00092D90
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
			}

			// Token: 0x06001611 RID: 5649 RVA: 0x00094BA0 File Offset: 0x00092DA0
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x06001612 RID: 5650 RVA: 0x00094BA4 File Offset: 0x00092DA4
			public static void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("inactive");
				simulated.InteractionState.SetInteractions(false, false, true, false, null, null);
				simulated.InteractionState.PushControls(new List<IControlBinding>
				{
					new Session.ClearDebrisControl(simulated)
				});
			}
		}

		// Token: 0x020002CF RID: 719
		public class ClearingState : Simulated.StateAction
		{
			// Token: 0x06001614 RID: 5652 RVA: 0x00094BF4 File Offset: 0x00092DF4
			public void Enter(Simulation simulation, Simulated simulated)
			{
				ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
				if (!entity.HasStartedClearing)
				{
					simulation.soundEffectManager.PlaySound("StartDebrisClearing");
					entity.ClearCompleteTime = new ulong?(TFUtils.EpochTime() + entity.ClearTime);
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), entity.ClearTime);
				}
				else
				{
					simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id, null);
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), entity.RemainingTime(TFUtils.EpochTime()));
				}
				simulated.command = null;
				Simulated.Debris.ClearingState.Setup(simulation, simulated);
			}

			// Token: 0x06001615 RID: 5653 RVA: 0x00094CB4 File Offset: 0x00092EB4
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.ClearSimulateOnce();
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.InteractionState.Clear();
			}

			// Token: 0x06001616 RID: 5654 RVA: 0x00094CD4 File Offset: 0x00092ED4
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.SimulateOnce();
				return false;
			}

			// Token: 0x06001617 RID: 5655 RVA: 0x00094CE0 File Offset: 0x00092EE0
			public static void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("clearing");
				simulated.DisplayThoughtState("clearing", simulation);
				simulated.InteractionState.SetInteractions(false, false, true, true, null, null);
				simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
			}
		}

		// Token: 0x020002D0 RID: 720
		public class ClearingMoreInfoState : Simulated.StateAction
		{
			// Token: 0x06001619 RID: 5657 RVA: 0x00094D2C File Offset: 0x00092F2C
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulation.soundEffectManager.PlaySound("StartDebrisClearing");
				ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"Debris(",
					simulated.Id.Describe(),
					"):ClearingMoreInfo. Ready in ",
					entity.ClearTime
				}));
				simulated.command = null;
				simulated.DisplayState("clearing_more_info");
				simulated.DisplayThoughtState("clearing_more_info", simulation);
				simulated.InteractionState.SetInteractions(false, false, true, true, null, null);
				IDisplayController thoughtItemBubbleDisplayController = simulated.thoughtItemBubbleDisplayController;
				SBGUILabel dynamicThinkingLabel = simulated.DynamicThinkingLabel;
				thoughtItemBubbleDisplayController.AttachGUIElementToTarget(dynamicThinkingLabel, "BN_CLOCK_TIME");
				dynamicThinkingLabel.transform.rotation = thoughtItemBubbleDisplayController.Transform.rotation;
				dynamicThinkingLabel.transform.localScale = new Vector3(-10f, 10f, -1f);
				simulated.ThinkingGhostButton.ClickEvent += delegate()
				{
					if (!simulation.Whitelisted || simulation.CheckWhitelist(simulated))
					{
						simulation.Router.Send(RushCommand.Create(simulated.Id));
					}
				};
				ulong delay = Math.Min(this.CalculateRemainingTime(simulated), 8UL);
				simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id), delay);
			}

			// Token: 0x0600161A RID: 5658 RVA: 0x00094EC4 File Offset: 0x000930C4
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.Router.CancelMatching(Command.TYPE.ABORT, simulated.Id, simulated.Id, null);
				simulated.DisplayState("clearing");
				simulated.DisplayThoughtState("clearing", simulation);
				simulated.RemoveDynamicThinkingElements();
				simulated.ClearSimulateOnce();
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.InteractionState.Clear();
			}

			// Token: 0x0600161B RID: 5659 RVA: 0x00094F28 File Offset: 0x00093128
			public ulong CalculateRemainingTime(Simulated simulated)
			{
				ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
				ulong utcNow = TFUtils.EpochTime();
				return entity.RemainingTime(utcNow);
			}

			// Token: 0x0600161C RID: 5660 RVA: 0x00094F4C File Offset: 0x0009314C
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				string text = string.Format(TFUtils.DurationToString(this.CalculateRemainingTime(simulated)), new object[0]);
				simulated.DynamicThinkingLabel.Text = text;
				SBGUIButton thinkingGhostButton = simulated.ThinkingGhostButton;
				thinkingGhostButton.SetParent(null);
				thinkingGhostButton.SetScreenPosition(simulation.ScreenPositionFromWorldPosition(simulated.thoughtItemBubbleDisplayController.Position));
				thinkingGhostButton.UpdateCollider();
				return false;
			}
		}

		// Token: 0x020002D1 RID: 721
		public class PrimingRushState : Simulated.StateActionDefault
		{
			// Token: 0x0600161E RID: 5662 RVA: 0x00094FB4 File Offset: 0x000931B4
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				ClearableDecorator clearable = simulated.GetEntity<ClearableDecorator>();
				ulong started = clearable.ClearCompleteTime.Value - clearable.ClearTime;
				Action<Session> execute = delegate(Session session)
				{
					simulation.Router.Send(RushCommand.Create(simulated.Id));
					int nJellyCost = 0;
					Cost.Prorate(clearable.ClearingRushCost, started, clearable.ClearCompleteTime.Value, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out nJellyCost);
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, clearable.DefinitionId, nJellyCost, "Accelerate_" + clearable.Name, "debris", "speedup", "debris", "confirm");
				};
				Action<Session> cancel = delegate(Session session)
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					int num = 0;
					Cost.Prorate(clearable.ClearingRushCost, started, clearable.ClearCompleteTime.Value, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out num);
				};
				simulated.rushParameters = new Simulated.RushParameters(execute, cancel, (ulong time) => Cost.Prorate(clearable.ClearingRushCost, started, clearable.ClearCompleteTime.Value, time), clearable.BlueprintName, clearable.DefinitionId, delegate(Session session, Cost cost, bool canAfford)
				{
					this.LogRush(session, simulated, cost, canAfford);
				}, simulation.ScreenPositionFromWorldPosition(simulated.ThoughtDisplayController.Position));
			}

			// Token: 0x0600161F RID: 5663 RVA: 0x00095094 File Offset: 0x00093294
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				session.AddAsyncResponse("request_rush_sim", simulated);
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				return false;
			}

			// Token: 0x06001620 RID: 5664 RVA: 0x000950CC File Offset: 0x000932CC
			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
				session.analytics.LogRushClear(simulated.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
			}
		}

		// Token: 0x020002D2 RID: 722
		public class RushingClearingState : Simulated.RushingSomething
		{
			// Token: 0x06001622 RID: 5666 RVA: 0x0009510C File Offset: 0x0009330C
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
				TFUtils.Assert(entity.ClearCompleteTime != null, "Should be setting up the clear complete time before we get here.");
				simulated.CalculateRushCompletionPercent(entity.ClearCompleteTime.Value, entity.ClearTime);
				entity.ClearCompleteTime = new ulong?(TFUtils.EpochTime());
				base.Enter(simulation, simulated);
			}

			// Token: 0x06001623 RID: 5667 RVA: 0x00095174 File Offset: 0x00093374
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				Cost cost = new Cost();
				cost += simulated.GetEntity<ClearableDecorator>().ClearingRushCost;
				cost.Prorate((float)simulated.Variable[Simulated.RUSH_PERCENT]);
				simulation.ModifyGameStateSimulated(simulated, new RushDebrisAction(simulated.Id, cost, simulated.GetEntity<ClearableDecorator>().ClearCompleteTime.Value));
			}

			// Token: 0x06001624 RID: 5668 RVA: 0x000951E0 File Offset: 0x000933E0
			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<ClearableDecorator>().ClearingRushCost;
			}
		}

		// Token: 0x020002D3 RID: 723
		public class DeletingState : Simulated.StateAction
		{
			// Token: 0x06001626 RID: 5670 RVA: 0x000951F8 File Offset: 0x000933F8
			public void Enter(Simulation simulation, Simulated simulated)
			{
				Simulated.Debris.DeletingState.Setup(simulation, simulated);
			}

			// Token: 0x06001627 RID: 5671 RVA: 0x00095204 File Offset: 0x00093404
			public void Leave(Simulation simulation, Simulated simulated)
			{
				this.SpawnDrops(simulation, simulated);
			}

			// Token: 0x06001628 RID: 5672 RVA: 0x00095210 File Offset: 0x00093410
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x06001629 RID: 5673 RVA: 0x00095214 File Offset: 0x00093414
			public static void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.DisplayState("deleting");
				simulated.DisplayThoughtState(simulated.GetEntity<ClearableDecorator>().ClearingReward.Summary.ThoughtIcon, "deleting", simulation);
				simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
			}

			// Token: 0x0600162A RID: 5674 RVA: 0x00095274 File Offset: 0x00093474
			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
				Reward reward = this.GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow, false);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Debris.DeletingState.SpawnDrops - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
				DebrisCompleteAction debrisCompleteAction = new DebrisCompleteAction(simulated.Id, reward);
				debrisCompleteAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, debrisCompleteAction);
				debrisCompleteAction.AddPickup(simulation);
				simulation.soundEffectManager.PlaySound("PopResourceBubble");
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Pop", 0, 0, 0f, simulated.thoughtBubblePopParticleRequestDelegate);
			}

			// Token: 0x0600162B RID: 5675 RVA: 0x00095334 File Offset: 0x00093534
			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<ClearableDecorator>().ClearingReward.GenerateReward(simulation, false);
			}
		}

		// Token: 0x020002D4 RID: 724
		public class DeletedState : Simulated.StateActionDefault
		{
			// Token: 0x0600162D RID: 5677 RVA: 0x00095350 File Offset: 0x00093550
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState(null);
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.Clear();
			}

			// Token: 0x0600162E RID: 5678 RVA: 0x00095374 File Offset: 0x00093574
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return true;
			}
		}

		// Token: 0x020004AE RID: 1198
		// (Invoke) Token: 0x0600251B RID: 9499
		public delegate void Setup(Simulation simulation, Simulated simulated);
	}

	// Token: 0x020002D5 RID: 725
	public class Disabled
	{
		// Token: 0x04000F24 RID: 3876
		public static Simulated.Disabled.DisabledState Disable = new Simulated.Disabled.DisabledState();

		// Token: 0x020002D6 RID: 726
		public class DisabledState : Simulated.StateActionDefault
		{
			// Token: 0x06001632 RID: 5682 RVA: 0x00095394 File Offset: 0x00093594
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
			}

			// Token: 0x06001633 RID: 5683 RVA: 0x000953A0 File Offset: 0x000935A0
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return true;
			}
		}
	}

	// Token: 0x020002D7 RID: 727
	public abstract class FollowingPath
	{
		// Token: 0x06001635 RID: 5685 RVA: 0x000953AC File Offset: 0x000935AC
		public bool PathFound(Simulation simulation, Simulated simulated)
		{
			return null != simulated.Variable["path"];
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x000953C4 File Offset: 0x000935C4
		public void FindPath(Simulation simulation, Simulated simulated)
		{
			TerrainPathing terrainPathing = simulated.Variable["pathing"] as TerrainPathing;
			if (terrainPathing == null)
			{
				return;
			}
			simulated.Variable["pathGoal"] = terrainPathing.Goal;
			PathFinder2.PROGRESS progress = terrainPathing.Seek(200);
			if (progress != PathFinder2.PROGRESS.SEEKING)
			{
				if (progress == PathFinder2.PROGRESS.DONE)
				{
					Path<GridPosition> path;
					terrainPathing.BuildPath(out path);
					path.Begin();
					simulated.ClearPathInfo();
					simulated.Variable["path"] = path;
					simulated.Variable["pathing"] = null;
					simulated.Variable["speed.variance"] = Simulated.FollowingPath.GetMovespeedVariance();
				}
				else if (progress == PathFinder2.PROGRESS.FAILED)
				{
					simulated.commands.Clear();
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
				}
			}
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x000954A4 File Offset: 0x000936A4
		public bool FollowPath(Simulation simulation, Simulated simulated)
		{
			bool result = false;
			Path<GridPosition> path = simulated.Variable["path"] as Path<GridPosition>;
			if (path.Done())
			{
				simulated.Warp(simulated.position[1], null);
				result = true;
			}
			else
			{
				Vector2 a = simulation.Terrain.ComputeWorldPosition(path.Current);
				Vector2 a2 = a - simulated.Position;
				float sqrMagnitude = a2.sqrMagnitude;
				if (sqrMagnitude < 100f)
				{
					path.Next();
					result = this.FollowPath(simulation, simulated);
				}
				else
				{
					float num = Mathf.Sqrt(sqrMagnitude);
					a2 /= num;
					simulated.Position += Mathf.Min(num, simulation.TimeStep * ((float)simulated.Variable["speed"] + this.GetSpeedAddition(simulated)) * (float)simulated.Variable["speed.variance"]) * a2;
				}
			}
			return result;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x000955A8 File Offset: 0x000937A8
		public bool FollowPathSimulate(Simulation simulation, Simulated simulated)
		{
			if (this.PathFound(simulation, simulated))
			{
				if (this.FollowPath(simulation, simulated))
				{
					simulated.command = null;
					return true;
				}
			}
			else
			{
				this.FindPath(simulation, simulated);
			}
			return false;
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x000955E8 File Offset: 0x000937E8
		public static void GetWaypointPath(Simulation simulation, Simulated simulated)
		{
			int num = 100;
			simulated.Variable["pathing"] = null;
			simulated.ClearPathInfo();
			Waypoint waypoint = null;
			while (waypoint == null && num > 0)
			{
				waypoint = simulation.GetRandomWaypoint();
				num--;
			}
			if (waypoint != null)
			{
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, waypoint.Position);
			}
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00095658 File Offset: 0x00093858
		public void RandomWanderSimulate(Simulation simulation, Simulated simulated)
		{
			if (!simulated.Variable.ContainsKey("path") || (simulated.Variable["path"] == null && simulated.Variable["pathing"] == null) || this.FollowPathSimulate(simulation, simulated))
			{
				Simulated.FollowingPath.GetWaypointPath(simulation, simulated);
			}
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x000956B8 File Offset: 0x000938B8
		private static float GetMovespeedVariance()
		{
			return UnityEngine.Random.Range(0.95f, 1.05f);
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000956CC File Offset: 0x000938CC
		protected virtual float GetSpeedAddition(Simulated simulated)
		{
			return 0f;
		}

		// Token: 0x04000F25 RID: 3877
		private const float TOL = 10f;

		// Token: 0x04000F26 RID: 3878
		private const float TOLSQ = 100f;

		// Token: 0x04000F27 RID: 3879
		private const int PATHING_BUDGET = 200;
	}

	// Token: 0x020002D8 RID: 728
	public class Landmark
	{
		// Token: 0x0600163F RID: 5695 RVA: 0x000956FC File Offset: 0x000938FC
		public static Simulated Load(LandmarkEntity landmarkEntity, Simulation simulation, Vector2 position, ulong utcNow)
		{
			string key;
			if (!landmarkEntity.GetDecorator<PurchasableDecorator>().Purchased)
			{
				key = "unpurchased";
			}
			else if (landmarkEntity.GetDecorator<ActivatableDecorator>().Activated == 0UL)
			{
				key = "inactive";
			}
			else
			{
				key = "active";
			}
			Simulated simulated = simulation.CreateSimulated(landmarkEntity, EntityManager.LandmarkActions[key], position);
			simulated.Warp(position, simulation);
			simulated.Visible = true;
			simulated.SetFootprint(simulation, true);
			return simulated;
		}

		// Token: 0x04000F28 RID: 3880
		public static Simulated.Landmark.UnpurchasedState Unpurchased = new Simulated.Landmark.UnpurchasedState();

		// Token: 0x04000F29 RID: 3881
		public static Simulated.Landmark.InactiveState Inactive = new Simulated.Landmark.InactiveState();

		// Token: 0x04000F2A RID: 3882
		public static Simulated.Landmark.ActiveState Active = new Simulated.Landmark.ActiveState();

		// Token: 0x020002D9 RID: 729
		public class UnpurchasedState : Simulated.StateAction
		{
			// Token: 0x06001641 RID: 5697 RVA: 0x00095780 File Offset: 0x00093980
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Simulated.Landmark.UnpurchasedState.Setup(simulated, simulation);
			}

			// Token: 0x06001642 RID: 5698 RVA: 0x00095790 File Offset: 0x00093990
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.Variable["purchased"] = true;
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}

			// Token: 0x06001643 RID: 5699 RVA: 0x000957D4 File Offset: 0x000939D4
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x06001644 RID: 5700 RVA: 0x000957D8 File Offset: 0x000939D8
			public static void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.DisplayState("inactive");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}
		}

		// Token: 0x020002DA RID: 730
		public class InactiveState : Simulated.StateAction
		{
			// Token: 0x06001646 RID: 5702 RVA: 0x00095800 File Offset: 0x00093A00
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Simulated.Landmark.InactiveState.Setup(simulated, simulation);
			}

			// Token: 0x06001647 RID: 5703 RVA: 0x00095810 File Offset: 0x00093A10
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}

			// Token: 0x06001648 RID: 5704 RVA: 0x00095830 File Offset: 0x00093A30
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x06001649 RID: 5705 RVA: 0x00095834 File Offset: 0x00093A34
			public static void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.DisplayState("inactive");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}
		}

		// Token: 0x020002DB RID: 731
		public class ActiveState : Simulated.StateAction
		{
			// Token: 0x0600164B RID: 5707 RVA: 0x0009585C File Offset: 0x00093A5C
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Simulated.Landmark.ActiveState.Setup(simulated, simulation);
			}

			// Token: 0x0600164C RID: 5708 RVA: 0x0009586C File Offset: 0x00093A6C
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}

			// Token: 0x0600164D RID: 5709 RVA: 0x0009588C File Offset: 0x00093A8C
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x0600164E RID: 5710 RVA: 0x00095890 File Offset: 0x00093A90
			public static void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.DisplayState("active");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}
		}

		// Token: 0x020004AF RID: 1199
		// (Invoke) Token: 0x0600251F RID: 9503
		public delegate void Setup(Simulated simulated, Simulation simulation);
	}

	// Token: 0x020002DC RID: 732
	public class Resident
	{
		// Token: 0x06001651 RID: 5713 RVA: 0x00095A10 File Offset: 0x00093C10
		public static Simulated Load(ResidentEntity residentEntity, Identity residenceId, ulong? wishExpiresAt, int? hungerId, int? prevHungerId, ulong nextHungerTime, ulong? fullnessLength, Reward matchBonus, Simulation simulation, ulong utcNow)
		{
			residentEntity.Residence = residenceId;
			residentEntity.HungryAt = nextHungerTime;
			if (fullnessLength != null)
			{
				residentEntity.FullnessLength = fullnessLength.Value;
			}
			else
			{
				residentEntity.FullnessLength = nextHungerTime - TFUtils.EpochTime();
			}
			residentEntity.FullnessRushCostFull = ResourceManager.CalculateFullnessRushCost(residentEntity.FullnessLength);
			residentEntity.HungerResourceId = hungerId;
			residentEntity.PreviousResourceId = prevHungerId;
			residentEntity.WishExpiresAt = wishExpiresAt;
			residentEntity.MatchBonus = matchBonus;
			if (residentEntity.CostumeDID == null && residentEntity.DefaultCostumeDID != null)
			{
				residentEntity.CostumeDID = residentEntity.DefaultCostumeDID;
			}
			Simulated.StateAction stateAction = EntityManager.ResidentActions["idle"];
			List<uint> bonusPaytableIds = residentEntity.BonusPaytableIds;
			PaytableManager paytableManager = simulation.game.paytableManager;
			Paytable[] array;
			if (residentEntity.JoinPaytables.Value)
			{
				Paytable paytable = null;
				foreach (uint did in bonusPaytableIds)
				{
					paytable = paytableManager.Get(did).Join(paytable);
				}
				paytable.Normalize();
				array = new Paytable[]
				{
					paytable
				};
			}
			else
			{
				int count = bonusPaytableIds.Count;
				array = new Paytable[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = paytableManager.Get(bonusPaytableIds[i]);
				}
			}
			residentEntity.BonusPaytables = array;
			TFUtils.DebugLog(string.Concat(new object[]
			{
				"Loading resident(name=",
				(string)residentEntity.Invariable["name"],
				", id=",
				residentEntity.Id,
				", did=",
				residentEntity.DefinitionId,
				", state=",
				stateAction.ToString(),
				", nextHungerTime=",
				nextHungerTime,
				"(",
				nextHungerTime - TFUtils.EpochTime(),
				" seconds from now), hungerResourceId=",
				hungerId,
				", wishExpiresAt=",
				wishExpiresAt,
				"(",
				(wishExpiresAt == null) ? null : new ulong?(wishExpiresAt.Value - TFUtils.EpochTime()),
				" seconds from now), residenceId=",
				residenceId,
				", loadedPaytableCount=",
				bonusPaytableIds.Count
			}));
			Simulated simulated = simulation.FindSimulated(residenceId);
			if (simulated == null)
			{
				if (!SoaringInternal.IsProductionMode)
				{
					TFUtils.Assert(simulated != null, "Don't know where to place the resident since there is no residence that owns it.");
				}
				return null;
			}
			Simulated simulated2 = simulation.CreateSimulated(residentEntity, stateAction, simulated.PointOfInterest);
			simulated2.Warp(simulated.PointOfInterest, simulation);
			simulated2.Visible = true;
			simulated2.IsSwarmManaged = true;
			SwarmManager.Instance.AddResident(residentEntity);
			List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated2.entity.DefinitionId, simulated2.Id, true);
			if (activeTasksForSimulated.Count > 0)
			{
				if (activeTasksForSimulated[0].m_pTaskData.m_eTaskType == TaskData._eTaskType.eEnter)
				{
					if (residentEntity.MatchBonus != null)
					{
						simulated2.EnterInitialState(EntityManager.ResidentActions["task_enter_feed"], simulation);
					}
					else
					{
						simulated2.EnterInitialState(EntityManager.ResidentActions["task_enter"], simulation);
					}
				}
				else if (activeTasksForSimulated[0].m_pTaskData.m_eTaskType == TaskData._eTaskType.eStand || activeTasksForSimulated[0].m_pTaskData.m_eTaskType == TaskData._eTaskType.eActivate)
				{
					simulated2.EnterInitialState(EntityManager.ResidentActions["task_stand"], simulation);
				}
				else
				{
					simulated2.EnterInitialState(EntityManager.ResidentActions["task_delegating"], simulation);
				}
			}
			else if (residentEntity.MatchBonus != null)
			{
				simulated2.EnterInitialState(EntityManager.ResidentActions["wait_bonus"], simulation);
			}
			else
			{
				simulated2.EnterInitialState(EntityManager.ResidentActions["start_wander"], simulation);
			}
			Simulated.Resident.SanityChecks(residentEntity, simulation.game);
			if (residentEntity.CostumeDID != null)
			{
				CostumeManager.Costume costume = simulation.game.costumeManager.GetCostume(residentEntity.CostumeDID.Value);
				simulated2.SetCostume(costume);
			}
			return simulated2;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00095EC4 File Offset: 0x000940C4
		private static void SanityChecks(ResidentEntity residentEntity, Game game)
		{
			CdfDictionary<int> wishTable = game.wishTableManager.GetWishTable(residentEntity.WishTableDID);
			Simulated.Resident.WishTableSanityCheck(residentEntity, wishTable, null);
			List<CostumeManager.Costume> costumesForUnit = game.costumeManager.GetCostumesForUnit(residentEntity.DefinitionId, true, true);
			int count = costumesForUnit.Count;
			for (int i = 0; i < count; i++)
			{
				CostumeManager.Costume costume = costumesForUnit[i];
				if (costume.m_nWishTableDID >= 0)
				{
					Simulated.Resident.WishTableSanityCheck(residentEntity, game.wishTableManager.GetWishTable(costume.m_nWishTableDID), costume);
				}
			}
			int? defaultCostumeDID = residentEntity.DefaultCostumeDID;
			if (defaultCostumeDID != null && !game.costumeManager.IsCostumeValidForUnit(residentEntity.DefinitionId, defaultCostumeDID.Value))
			{
				TFUtils.Assert(false, string.Format("The resident({0}) has a default costume did of {1} but no costume with that did exists for this resident.", residentEntity.BlueprintName, defaultCostumeDID.Value));
			}
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00095F9C File Offset: 0x0009419C
		private static void WishTableSanityCheck(ResidentEntity pResidentEntity, CdfDictionary<int> pWishTable, CostumeManager.Costume pCostume)
		{
			if (pWishTable == null)
			{
				string text = "Resident did: " + pResidentEntity.DefinitionId.ToString() + " has an undefined wishtable did: " + pResidentEntity.WishTableDID.ToString();
				if (pCostume != null)
				{
					text = text + " for costume did: " + pCostume.m_nDID;
				}
				TFUtils.Assert(false, text);
				return;
			}
			if (pCostume == null)
			{
				pWishTable.Validate(true, string.Format("The resident{0})", pResidentEntity.BlueprintName));
			}
			else
			{
				pWishTable.Validate(true, string.Format("The costume did:{0})", pCostume.m_nDID));
			}
			foreach (int num in pWishTable.ValuesClone)
			{
				int num2 = pResidentEntity.BonusPaytables.Length;
				bool flag = false;
				for (int i = 0; i < num2; i++)
				{
					flag = (pResidentEntity.BonusPaytables[i].CanWager((uint)num) || flag);
					pResidentEntity.BonusPaytables[i].ValidateProbabilities();
				}
				if (!flag)
				{
					string text;
					if (pCostume == null)
					{
						text = string.Format("The resident({0}) can wish for product({1}) but has not bonus paytable to reward it!", pResidentEntity.BlueprintName, num);
					}
					else
					{
						text = string.Format("The resident({0}) using costume({1}) can wish for product({2}) but has not bonus paytable to reward it!", pResidentEntity.BlueprintName, pCostume.m_nDID, num);
					}
					TFUtils.Assert(false, text);
				}
			}
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00096130 File Offset: 0x00094330
		private static void StartHungerTimer(ResidentEntity resident, Simulation simulation)
		{
			resident.HungryAt = TFUtils.EpochTime() + resident.FullnessLength;
			simulation.Router.Send(HungerCommand.Create(resident.Id, resident.Id), resident.FullnessLength);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00096174 File Offset: 0x00094374
		public static void RefreshModifiedDisplayState(Simulated simulated)
		{
			if (simulated.StateModifierString != null)
			{
				string text = simulated.GetDisplayState();
				if (text != null)
				{
					text = text.Replace(string.Format(simulated.StateModifierString, string.Empty), string.Empty);
					simulated.StateModifierString = null;
					simulated.DisplayState(text);
				}
			}
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x000961C4 File Offset: 0x000943C4
		private static int? GenerateHungerResourceID(Simulation pSimulation, ResidentEntity pEntity)
		{
			int? result = null;
			if (pEntity == null)
			{
				return null;
			}
			CdfDictionary<int> cdfDictionary = null;
			if (pEntity.CostumeDID != null)
			{
				CostumeManager.Costume costume = pSimulation.game.costumeManager.GetCostume(pEntity.CostumeDID.Value);
				if (costume != null && costume.m_nWishTableDID >= 0)
				{
					cdfDictionary = pSimulation.game.wishTableManager.GetWishTable(costume.m_nWishTableDID);
				}
			}
			if (cdfDictionary == null)
			{
				cdfDictionary = pSimulation.game.wishTableManager.GetWishTable(pEntity.WishTableDID);
			}
			if (cdfDictionary != null)
			{
				if (pEntity.PreviousResourceId == 9100)
				{
					cdfDictionary = cdfDictionary.Where((int productId) => productId != 9100, true);
				}
				if (!pSimulation.featureManager.CheckFeature("resident_wishes_full_pool"))
				{
					ICollection<int> craftableProducts = pSimulation.craftManager.UnlockedProductsDeepCopy;
					cdfDictionary = cdfDictionary.Where((int productId) => craftableProducts.Contains(productId), true);
				}
				result = new int?(cdfDictionary.Spin(ResourceManager.DEFAULT_WISH));
			}
			else
			{
				TFUtils.DebugLog("error - entity missing wishpool: " + pEntity.BlueprintName);
			}
			return result;
		}

		// Token: 0x04000F2B RID: 3883
		public static Simulated.Resident.IdleState Idle = new Simulated.Resident.IdleState();

		// Token: 0x04000F2C RID: 3884
		public static Simulated.Resident.IdleFullState IdleFull = new Simulated.Resident.IdleFullState();

		// Token: 0x04000F2D RID: 3885
		public static Simulated.Resident.IdleWishingState IdleWishing = new Simulated.Resident.IdleWishingState();

		// Token: 0x04000F2E RID: 3886
		public static Simulated.Resident.MovingState Moving = new Simulated.Resident.MovingState();

		// Token: 0x04000F2F RID: 3887
		public static Simulated.Resident.GoHomeState GoHome = new Simulated.Resident.GoHomeState();

		// Token: 0x04000F30 RID: 3888
		public static Simulated.Resident.StoreResidentState StoreResident = new Simulated.Resident.StoreResidentState();

		// Token: 0x04000F31 RID: 3889
		public static Simulated.Resident.ResidingState Residing = new Simulated.Resident.ResidingState();

		// Token: 0x04000F32 RID: 3890
		public static Simulated.Resident.WanderingFullState WanderingFull = new Simulated.Resident.WanderingFullState();

		// Token: 0x04000F33 RID: 3891
		public static Simulated.Resident.WanderingHungryState WanderingHungry = new Simulated.Resident.WanderingHungryState();

		// Token: 0x04000F34 RID: 3892
		public static Simulated.Resident.WishingForFoodState WishingForFood = new Simulated.Resident.WishingForFoodState();

		// Token: 0x04000F35 RID: 3893
		public static Simulated.Resident.TemptedState Tempted = new Simulated.Resident.TemptedState();

		// Token: 0x04000F36 RID: 3894
		public static Simulated.Resident.NotTemptedState NotTempted = new Simulated.Resident.NotTemptedState();

		// Token: 0x04000F37 RID: 3895
		public static Simulated.Resident.PrimingRushFullnessState PrimingRushFullness = new Simulated.Resident.PrimingRushFullnessState();

		// Token: 0x04000F38 RID: 3896
		public static Simulated.Resident.RushingFullnessState RushingFullness = new Simulated.Resident.RushingFullnessState();

		// Token: 0x04000F39 RID: 3897
		public static Simulated.Resident.TryEatState TryEat = new Simulated.Resident.TryEatState();

		// Token: 0x04000F3A RID: 3898
		public static Simulated.Resident.WaitingForDeliveryState WaitingForDelivery = new Simulated.Resident.WaitingForDeliveryState();

		// Token: 0x04000F3B RID: 3899
		public static Simulated.Resident.CheeringState Cheering = new Simulated.Resident.CheeringState();

		// Token: 0x04000F3C RID: 3900
		public static Simulated.Resident.EatingState Eating = new Simulated.Resident.EatingState();

		// Token: 0x04000F3D RID: 3901
		public static Simulated.Resident.TryBonusSpinState TryBonusSpin = new Simulated.Resident.TryBonusSpinState();

		// Token: 0x04000F3E RID: 3902
		public static Simulated.Resident.WaitingForCollectBonusState WaitingForCollectBonus = new Simulated.Resident.WaitingForCollectBonusState();

		// Token: 0x04000F3F RID: 3903
		public static Simulated.Resident.CheeringAfterBonusState CheeringAfterBonus = new Simulated.Resident.CheeringAfterBonusState();

		// Token: 0x04000F40 RID: 3904
		public static Simulated.Resident.StartingWanderCycleState StartingWanderCycle = new Simulated.Resident.StartingWanderCycleState();

		// Token: 0x04000F41 RID: 3905
		public static Simulated.Resident.RequestingInterfaceState RequestingInterface = new Simulated.Resident.RequestingInterfaceState();

		// Token: 0x04000F42 RID: 3906
		public static Simulated.Resident.ReflectingState Reflecting = new Simulated.Resident.ReflectingState();

		// Token: 0x04000F43 RID: 3907
		public static Simulated.Resident.TaskDelegatingState TaskDelegating = new Simulated.Resident.TaskDelegatingState();

		// Token: 0x04000F44 RID: 3908
		public static Simulated.Resident.TaskIdleState TaskIdle = new Simulated.Resident.TaskIdleState();

		// Token: 0x04000F45 RID: 3909
		public static Simulated.Resident.TaskWanderState TaskWander = new Simulated.Resident.TaskWanderState();

		// Token: 0x04000F46 RID: 3910
		public static Simulated.Resident.TaskMovingState TaskMoving = new Simulated.Resident.TaskMovingState();

		// Token: 0x04000F47 RID: 3911
		public static Simulated.Resident.TaskEnterState TaskEnter = new Simulated.Resident.TaskEnterState();

		// Token: 0x04000F48 RID: 3912
		public static Simulated.Resident.TaskEnterFeedState TaskEnterFeed = new Simulated.Resident.TaskEnterFeedState();

		// Token: 0x04000F49 RID: 3913
		public static Simulated.Resident.TaskStandState TaskStand = new Simulated.Resident.TaskStandState();

		// Token: 0x04000F4A RID: 3914
		public static Simulated.Resident.TaskCollectRewardState TaskCollectReward = new Simulated.Resident.TaskCollectRewardState();

		// Token: 0x04000F4B RID: 3915
		public static Simulated.Resident.TaskCheerAfterCollectState TaskCheerAfterCollect = new Simulated.Resident.TaskCheerAfterCollectState();

		// Token: 0x020002DD RID: 733
		public class IdleState : Simulated.StateAction
		{
			// Token: 0x06001659 RID: 5721 RVA: 0x00096340 File Offset: 0x00094540
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}

			// Token: 0x0600165A RID: 5722 RVA: 0x0009635C File Offset: 0x0009455C
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x0600165B RID: 5723 RVA: 0x00096360 File Offset: 0x00094560
			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x0600165C RID: 5724 RVA: 0x00096364 File Offset: 0x00094564
			public void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState("idle");
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
			}
		}

		// Token: 0x020002DE RID: 734
		public class IdleFullState : Simulated.Resident.IdleState
		{
			// Token: 0x0600165E RID: 5726 RVA: 0x000963A4 File Offset: 0x000945A4
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				simulated.GetEntity<ResidentEntity>().HomeAvailability = true;
				simulated.GetEntity<ResidentEntity>().StartCheckForResume();
				simulated.DisplayState("idle");
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.command = null;
			}

			// Token: 0x0600165F RID: 5727 RVA: 0x000963FC File Offset: 0x000945FC
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().StopCheckForResume();
				simulated.InteractionState.Clear();
				base.Leave(simulation, simulated);
			}

			// Token: 0x06001660 RID: 5728 RVA: 0x00096428 File Offset: 0x00094628
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				bool result = base.Simulate(simulation, simulated, session);
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return result;
				}
				if (simulated.GetEntity<ResidentEntity>().CheckForResume())
				{
					simulation.Router.Send(ResumeFullCommand.Create(simulated.Id, simulated.Id));
				}
				return result;
			}
		}

		// Token: 0x020002DF RID: 735
		public class IdleWishingState : Simulated.Resident.IdleState
		{
			// Token: 0x06001662 RID: 5730 RVA: 0x000964C4 File Offset: 0x000946C4
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				simulated.GetEntity<ResidentEntity>().HomeAvailability = true;
				simulated.GetEntity<ResidentEntity>().StartCheckForResume();
				simulated.DisplayState("idle");
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.command = null;
			}

			// Token: 0x06001663 RID: 5731 RVA: 0x0009651C File Offset: 0x0009471C
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().StopCheckForResume();
				simulated.InteractionState.Clear();
				base.Leave(simulation, simulated);
			}

			// Token: 0x06001664 RID: 5732 RVA: 0x00096548 File Offset: 0x00094748
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				bool result = base.Simulate(simulation, simulated, session);
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return result;
				}
				if (simulated.GetEntity<ResidentEntity>().CheckForResume())
				{
					simulation.Router.Send(ResumeWishingCommand.Create(simulated.Id, simulated.Id));
				}
				return result;
			}
		}

		// Token: 0x020002E0 RID: 736
		public class MovingState : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x06001666 RID: 5734 RVA: 0x000965E4 File Offset: 0x000947E4
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, (Vector2)simulated.command["position"]);
				simulated.ClearPathInfo();
				simulated.SimulatedQueryable = true;
				simulated.DisplayState("walk");
			}

			// Token: 0x06001667 RID: 5735 RVA: 0x00096648 File Offset: 0x00094848
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (base.FollowPathSimulate(simulation, simulated))
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}

			// Token: 0x06001668 RID: 5736 RVA: 0x00096680 File Offset: 0x00094880
			public void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		// Token: 0x020002E1 RID: 737
		public class GoHomeState : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x0600166A RID: 5738 RVA: 0x0009668C File Offset: 0x0009488C
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, (Vector2)simulated.command["home_position"]);
				simulated.ClearPathInfo();
				simulated.command = null;
				simulated.DisplayState("walk");
			}

			// Token: 0x0600166B RID: 5739 RVA: 0x000966F0 File Offset: 0x000948F0
			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x0600166C RID: 5740 RVA: 0x000966F4 File Offset: 0x000948F4
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (base.FollowPathSimulate(simulation, simulated))
				{
					simulation.Router.Send(StoreResidentCommand.Create(simulated.Entity.Id, simulated.Entity.Id));
				}
				return false;
			}
		}

		// Token: 0x020002E2 RID: 738
		public class StoreResidentState : Simulated.StateActionDefault
		{
			// Token: 0x0600166E RID: 5742 RVA: 0x00096740 File Offset: 0x00094940
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulated.ClearPathInfo();
				simulated.Variable["path"] = null;
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				SwarmManager.Instance.StoreResident(simulation, simulated.GetEntity<ResidentEntity>());
			}
		}

		// Token: 0x020002E3 RID: 739
		public class ResidingState : Simulated.StateActionDefault
		{
			// Token: 0x06001670 RID: 5744 RVA: 0x00096798 File Offset: 0x00094998
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				TFUtils.DebugLog(string.Concat(new string[]
				{
					"Resident(",
					simulated.Id.Describe(),
					"):Residing(",
					(simulated.command["residence"] as Identity).Describe(),
					")"
				}));
				simulated.command = null;
			}
		}

		// Token: 0x020002E4 RID: 740
		public class WanderingFullState : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x06001672 RID: 5746 RVA: 0x00096814 File Offset: 0x00094A14
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				ulong delay = Math.Max(0UL, this.CalculateRemainingFullnessTime(simulated));
				Command command = HungerCommand.Create(simulated.Id, simulated.Id);
				simulation.Router.CancelMatching(command.Type, command.Sender, command.Receiver, null);
				simulation.Router.Send(command, delay);
				simulated.DisplayState("walk");
				simulated.DisplayThoughtState(null, simulation);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.SimulatedQueryable = true;
				simulated.GetEntity<ResidentEntity>().StartCheckForIdle();
				simulated.GetEntity<ResidentEntity>().HomeAvailability = true;
			}

			// Token: 0x06001673 RID: 5747 RVA: 0x000968C4 File Offset: 0x00094AC4
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().StopCheckForIdle();
				simulated.Warp(simulated.Position, null);
				simulation.Router.CancelMatching(Command.TYPE.HUNGER, simulated.Id, simulated.Id, null);
				simulated.InteractionState.Clear();
			}

			// Token: 0x06001674 RID: 5748 RVA: 0x00096910 File Offset: 0x00094B10
			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return false;
				}
				if (simulated.GetEntity<ResidentEntity>().CheckForIdle())
				{
					simulated.Variable["pathing"] = null;
					simulated.ClearPathInfo();
					simulated.Warp(simulated.Position, null);
					simulation.Router.Send(IdlePauseCommand.Create(simulated.Id, simulated.Id));
				}
				else
				{
					base.RandomWanderSimulate(simulation, simulated);
				}
				return false;
			}

			// Token: 0x06001675 RID: 5749 RVA: 0x000969C8 File Offset: 0x00094BC8
			protected ulong CalculateRemainingFullnessTime(Simulated simulated)
			{
				return simulated.GetEntity<ResidentEntity>().HungryAt - TFUtils.EpochTime();
			}
		}

		// Token: 0x020002E5 RID: 741
		public class WanderingHungryState : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x06001677 RID: 5751 RVA: 0x000969E4 File Offset: 0x00094BE4
			public void Enter(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				ulong num;
				if (entity.HungerResourceId != null)
				{
					num = 0UL;
				}
				else
				{
					num = (ulong)((long)UnityEngine.Random.Range(entity.WishCooldownMin, entity.WishCooldownMax));
					entity.WishExpiresAt = new ulong?(TFUtils.EpochTime() + num + (ulong)((long)entity.WishDuration));
				}
				if (simulation.featureManager.CheckFeature("resident_wishes"))
				{
					simulation.Router.Send(WishCommand.Create(simulated.Id, simulated.Id), num);
				}
				else
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), 60UL);
				}
				Simulated.FollowingPath.GetWaypointPath(simulation, simulated);
				simulated.DisplayState("walk");
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}

			// Token: 0x06001678 RID: 5752 RVA: 0x00096AEC File Offset: 0x00094CEC
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayThoughtState(null, simulation);
				simulated.Warp(simulated.Position, null);
			}

			// Token: 0x06001679 RID: 5753 RVA: 0x00096B04 File Offset: 0x00094D04
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return false;
				}
				base.RandomWanderSimulate(simulation, simulated);
				return false;
			}
		}

		// Token: 0x020002E6 RID: 742
		public abstract class WishingForSomething : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x0600167B RID: 5755 RVA: 0x00096B70 File Offset: 0x00094D70
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				TFUtils.Assert(simulated.ThoughtMaskDisplayController != null, "Simulateds that wish for something must have a thought mask display controller. This one does not. Sim=" + simulated);
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.WishExpiresAt != null)
				{
					ulong? wishExpiresAt = entity.WishExpiresAt;
					if (wishExpiresAt == null || wishExpiresAt.Value > TFUtils.EpochTime())
					{
						goto IL_88;
					}
				}
				entity.WishExpiresAt = new ulong?(TFUtils.EpochTime() + (ulong)((long)entity.WishDuration));
				IL_88:
				Simulated.FollowingPath.GetWaypointPath(simulation, simulated);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.DisplayState("walk");
				entity.StartCheckForIdle();
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}

			// Token: 0x0600167C RID: 5756 RVA: 0x00096C48 File Offset: 0x00094E48
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().StopCheckForIdle();
				simulated.Warp(simulated.Position, null);
			}

			// Token: 0x0600167D RID: 5757 RVA: 0x00096C70 File Offset: 0x00094E70
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return false;
				}
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				TFUtils.Assert(entity.WishExpiresAt != null, "No expiration time set... something is wrong!");
				if (entity.CheckForIdle())
				{
					simulated.Variable["pathing"] = null;
					simulated.ClearPathInfo();
					simulated.Warp(simulated.Position, null);
					simulation.Router.Send(IdlePauseCommand.Create(simulated.Id, simulated.Id));
				}
				else
				{
					base.RandomWanderSimulate(simulation, simulated);
				}
				if (TFUtils.EpochTime() >= entity.WishExpiresAt.Value && entity.HungerResourceId != null && !simulation.resourceManager.Resources[entity.HungerResourceId.Value].IgnoreWishDurationTimer)
				{
					entity.PreviousResourceId = entity.HungerResourceId;
					entity.HungerResourceId = null;
					simulation.ModifyGameStateSimulated(simulated, new FailWishAction(simulated));
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					simulated.DisplayThoughtState(null, simulation);
					simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Thought_Pop", 0, 0, 0f, simulated.thoughtBubblePopParticleRequestDelegate);
				}
				return false;
			}
		}

		// Token: 0x020002E7 RID: 743
		public class WishingForFoodState : Simulated.Resident.WishingForSomething
		{
			// Token: 0x0600167F RID: 5759 RVA: 0x00096E10 File Offset: 0x00095010
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.HomeAvailability = true;
				simulated.SimulatedQueryable = true;
				int? hungerResourceId = entity.HungerResourceId;
				if (hungerResourceId != null)
				{
					CdfDictionary<int> cdfDictionary = null;
					if (entity.CostumeDID != null)
					{
						CostumeManager.Costume costume = simulation.game.costumeManager.GetCostume(entity.CostumeDID.Value);
						if (costume != null && costume.m_nWishTableDID >= 0)
						{
							cdfDictionary = simulation.game.wishTableManager.GetWishTable(costume.m_nWishTableDID);
						}
					}
					if (cdfDictionary == null)
					{
						cdfDictionary = simulation.game.wishTableManager.GetWishTable(entity.WishTableDID);
					}
					if (cdfDictionary != null)
					{
						cdfDictionary = cdfDictionary.Where((int productId) => productId == hungerResourceId.Value, true);
					}
					if (cdfDictionary == null || cdfDictionary.Count <= 0)
					{
						hungerResourceId = null;
					}
				}
				if (hungerResourceId == null)
				{
					hungerResourceId = Simulated.Resident.GenerateHungerResourceID(simulation, entity);
					if (hungerResourceId != null && (hungerResourceId.GetValueOrDefault() == ResourceManager.DEFAULT_WISH && hungerResourceId != null))
					{
						hungerResourceId = null;
						simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					}
					if (hungerResourceId == null)
					{
						entity.WishExpiresAt = 0UL;
					}
					else if (!simulation.resourceManager.Resources.ContainsKey(hungerResourceId.Value))
					{
						TFUtils.WarningLog("Missing resource: " + hungerResourceId.Value);
						TFUtils.DebugLog("Attempting to wish for a resource we do not have: " + hungerResourceId.Value);
						entity.WishExpiresAt = 0UL;
					}
					else
					{
						if (hungerResourceId.GetValueOrDefault() == 9100 && hungerResourceId != null)
						{
							if (simulated.StateModifierString == null)
							{
								simulated.StateModifierString = "jerky{0}";
								simulated.DisplayState(simulated.GetDisplayState());
							}
						}
						else
						{
							Simulated.Resident.RefreshModifiedDisplayState(simulated);
						}
						entity.HungerResourceId = hungerResourceId;
						simulation.ModifyGameStateSimulated(simulated, new NewWishAction(simulated.Id, hungerResourceId.Value, entity.PreviousResourceId, entity.WishExpiresAt.Value));
					}
				}
				int? forcedWish = simulated.forcedWish;
				if (forcedWish != null)
				{
					entity.HungerResourceId = simulated.forcedWish;
					hungerResourceId = simulated.forcedWish;
				}
				if (hungerResourceId != null)
				{
					Resource resource = simulation.resourceManager.Resources[hungerResourceId.Value];
					string resourceTexture = resource.GetResourceTexture();
					simulated.DisplayThoughtState(resourceTexture, "hungry", simulation);
				}
				else
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}
		}

		// Token: 0x020002E8 RID: 744
		public class TemptedState : Simulated.StateAction
		{
			// Token: 0x06001681 RID: 5761 RVA: 0x00097154 File Offset: 0x00095354
			public void Enter(Simulation simulation, Simulated simulated)
			{
				Command command = simulated.command;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				int value = simulated.GetEntity<ResidentEntity>().HungerResourceId.Value;
				int temptingWith = (int)command["product_id"];
				string resourceTexture = simulation.resourceManager.Resources[temptingWith].GetResourceTexture();
				int? num = new int?(entity.GrossItemsWishTableDID);
				int? num2 = new int?(entity.ForbiddenItemsWishTableDID);
				bool flag = false;
				bool flag2 = false;
				if (num != null)
				{
					CdfDictionary<int> cdfDictionary = simulation.game.wishTableManager.GetWishTable(entity.GrossItemsWishTableDID);
					if (cdfDictionary != null)
					{
						cdfDictionary = cdfDictionary.Where((int productID) => productID == temptingWith, true);
					}
					if (cdfDictionary != null && cdfDictionary.Count > 0)
					{
						flag = true;
					}
				}
				if (num2 != null)
				{
					CdfDictionary<int> cdfDictionary2 = simulation.game.wishTableManager.GetWishTable(entity.ForbiddenItemsWishTableDID);
					if (cdfDictionary2 != null)
					{
						cdfDictionary2 = cdfDictionary2.Where((int productID) => productID == temptingWith, true);
					}
					if (cdfDictionary2 != null && cdfDictionary2.Count > 0)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					simulated.showUnavailableIcon = true;
					simulated.DisplayState("gimme");
					simulated.DisplayThoughtState("empty.png", "acceptable", simulation);
					this.AttachLabelToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, Language.Get("!!FEED_NO_WAY_COMMENT"));
					this.AttachProductIconToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, resourceTexture);
					simulation.soundEffectManager.PlaySound("NotTempted");
					simulated.DisplayState("NotTempted");
				}
				else if (flag)
				{
					simulated.showUnavailableIcon = false;
					simulated.DisplayState("deny");
					simulated.DisplayThoughtState("empty.png", "acceptable", simulation);
					this.AttachLabelToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, Language.Get("!!FEED_YUCK_COMMENT"));
					this.AttachProductIconToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, resourceTexture);
					simulation.soundEffectManager.PlaySound("NotTempted");
				}
				else if (value == temptingWith)
				{
					simulated.showUnavailableIcon = false;
					simulated.DisplayState("gimme");
					simulated.DisplayThoughtState("empty.png", "gimme", simulation);
					this.AttachLabelToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, Language.Get("!!FEED_MATCH_COMMENT"));
					this.AttachProductIconToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, resourceTexture);
					simulation.soundEffectManager.PlaySound("VeryTempted");
				}
				else
				{
					simulated.showUnavailableIcon = false;
					simulated.DisplayState("acceptable");
					simulated.DisplayThoughtState("empty.png", "acceptable", simulation);
					this.AttachLabelToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, Language.Get("!!FEED_MISMATCH_COMMENT"));
					this.AttachProductIconToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, resourceTexture);
					simulation.soundEffectManager.PlaySound("SomewhatTempted");
				}
			}

			// Token: 0x06001682 RID: 5762 RVA: 0x00097444 File Offset: 0x00095644
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x06001683 RID: 5763 RVA: 0x00097448 File Offset: 0x00095648
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.RemoveDynamicThinkingElements();
			}

			// Token: 0x06001684 RID: 5764 RVA: 0x00097450 File Offset: 0x00095650
			private void AttachLabelToThoughtBubbleBone(Simulated simulated, IDisplayController target, string text)
			{
				SBGUILabel dynamicThinkingLabel = simulated.DynamicThinkingLabel;
				dynamicThinkingLabel.Text = text;
				this.AttachHelper(target, "BN_TYPE", dynamicThinkingLabel);
				dynamicThinkingLabel.transform.localScale = new Vector3(-10f, 10f, -3f);
			}

			// Token: 0x06001685 RID: 5765 RVA: 0x00097498 File Offset: 0x00095698
			private void AttachProductIconToThoughtBubbleBone(Simulated simulated, IDisplayController target, string textureOverride)
			{
				SBGUIAtlasImage dynamicThinkingIcon = simulated.DynamicThinkingIcon;
				dynamicThinkingIcon.SetTextureFromAtlas(textureOverride);
				this.AttachHelper(target, "BN_ITEM", dynamicThinkingIcon);
				dynamicThinkingIcon.transform.localScale = new Vector3(-4f, 4f, -1f);
			}

			// Token: 0x06001686 RID: 5766 RVA: 0x000974E0 File Offset: 0x000956E0
			private void AttachHelper(IDisplayController controller, string target, SBGUIElement element)
			{
				if (controller == null)
				{
					Debug.LogError("Cannot attach food icon to a null skeleton!");
					return;
				}
				controller.AttachGUIElementToTarget(element, target);
				element.transform.rotation = controller.Transform.rotation;
			}
		}

		// Token: 0x020002E9 RID: 745
		public class NotTemptedState : Simulated.Resident.TransitionallyAnimating
		{
			// Token: 0x06001688 RID: 5768 RVA: 0x00097524 File Offset: 0x00095724
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				TFUtils.Assert(entity.HungryAt > TFUtils.EpochTime(), "Unit entered NotTempted state, but it appears to be hungry. Something is probably wrong.");
				simulation.soundEffectManager.PlaySound("NotTempted");
				entity.HomeAvailability = false;
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), 3UL);
			}

			// Token: 0x17000305 RID: 773
			// (get) Token: 0x06001689 RID: 5769 RVA: 0x00097594 File Offset: 0x00095794
			protected override string DisplayStateName
			{
				get
				{
					return "deny";
				}
			}

			// Token: 0x17000306 RID: 774
			// (get) Token: 0x0600168A RID: 5770 RVA: 0x0009759C File Offset: 0x0009579C
			protected override string DisplayThoughtStateName
			{
				get
				{
					return null;
				}
			}

			// Token: 0x0600168B RID: 5771 RVA: 0x000975A0 File Offset: 0x000957A0
			protected override string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated)
			{
				return null;
			}

			// Token: 0x17000307 RID: 775
			// (get) Token: 0x0600168C RID: 5772 RVA: 0x000975A4 File Offset: 0x000957A4
			protected override int AnimationLength
			{
				get
				{
					return 2;
				}
			}
		}

		// Token: 0x020002EA RID: 746
		public class PrimingRushFullnessState : Simulated.StateActionDefault
		{
			// Token: 0x0600168E RID: 5774 RVA: 0x000975B0 File Offset: 0x000957B0
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.HomeAvailability = false;
				Action<Session> execute = delegate(Session session)
				{
					simulation.Router.Send(RushCommand.Create(simulated.Id));
					int nJellyCost = 0;
					entity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out nJellyCost);
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, entity.DefinitionId, nJellyCost, entity.Name, "characters", "speedup", "fullness", "confirm");
				};
				Action<Session> cancel = delegate(Session session)
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					int num = 0;
					entity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out num);
				};
				simulated.rushParameters = new Simulated.RushParameters(execute, cancel, (ulong time) => entity.FullnessRushCostNow(), entity.BlueprintName, entity.DefinitionId, delegate(Session session, Cost cost, bool canAfford)
				{
					this.LogRush(session, simulated, cost, canAfford);
				}, simulation.ScreenPositionFromWorldPosition(simulated.ThoughtDisplayController.Position));
			}

			// Token: 0x0600168F RID: 5775 RVA: 0x00097678 File Offset: 0x00095878
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				session.AddAsyncResponse("request_rush_sim", simulated);
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				return false;
			}

			// Token: 0x06001690 RID: 5776 RVA: 0x000976B0 File Offset: 0x000958B0
			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
				session.analytics.LogRushFullness(simulated.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
			}
		}

		// Token: 0x020002EB RID: 747
		public class RushingFullnessState : Simulated.RushingSomething
		{
			// Token: 0x06001692 RID: 5778 RVA: 0x000976F0 File Offset: 0x000958F0
			public override void CancelCurrentCommands(Simulation simulation, Simulated simulated)
			{
				simulation.Router.CancelMatching(Command.TYPE.HUNGER, simulated.Id, simulated.Id, null);
			}

			// Token: 0x06001693 RID: 5779 RVA: 0x00097718 File Offset: 0x00095918
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.SimulatedQueryable = true;
				simulated.CalculateRushCompletionPercent(entity.HungryAt, entity.FullnessLength);
				entity.HomeAvailability = false;
				base.Enter(simulation, simulated);
			}

			// Token: 0x06001694 RID: 5780 RVA: 0x00097754 File Offset: 0x00095954
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.CalculateRushCompletionPercent(entity.HungryAt, entity.FullnessLength);
				Cost rushCost = entity.FullnessRushCostNow();
				entity.HungryAt = TFUtils.EpochTime();
				simulation.Router.Send(HungerCommand.Create(simulated.Id, simulated.Id), 0UL);
				simulation.ModifyGameStateSimulated(simulated, new RushHungerAction(simulated.Id, rushCost, entity.HungryAt));
				simulated.timebarMixinArgs.hasTimebar = false;
			}

			// Token: 0x06001695 RID: 5781 RVA: 0x000977D4 File Offset: 0x000959D4
			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				return entity.FullnessRushCostFull;
			}
		}

		// Token: 0x020002EC RID: 748
		public class TryEatState : Simulated.StateAction
		{
			// Token: 0x06001697 RID: 5783 RVA: 0x000977F8 File Offset: 0x000959F8
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				Command command = simulated.command;
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.HomeAvailability = false;
				bool flag = false;
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				int num = entity.HungerResourceId.Value;
				bool flag2 = false;
				if (command.HasProperty("product_id"))
				{
					num = (int)command["product_id"];
					if (num == entity.HungerResourceId)
					{
						flag = true;
						if (!simulation.resourceManager.Resources[entity.HungerResourceId.Value].ForceNoWishPayout)
						{
							flag2 = true;
						}
						if (simulated.forcedWish == num)
						{
							simulated.forcedWish = null;
						}
					}
					else if ((entity.HungerResourceId != null && simulation.resourceManager.Resources[entity.HungerResourceId.Value].ForceWishMatch) || simulation.resourceManager.Resources[num].ForceWishMatch)
					{
						if (activeTasksForSimulated.Count > 0)
						{
							simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id), 0UL);
							return;
						}
						simulation.Router.Send(WishCommand.Create(simulated.Id, simulated.Id), 0UL);
						return;
					}
					else
					{
						flag = false;
						simulated.DisplayThoughtState("empty.png", "hungry", simulation);
					}
					entity.PreviousResourceId = entity.HungerResourceId;
					entity.HungerResourceId = new int?(num);
				}
				if (simulation.resourceManager.HasEnough(num, 1))
				{
					if (flag2)
					{
						this.GenerateAndRecordBonusEarned((uint)num, simulation, simulated);
					}
					if (simulation.resourceManager.Resources[num].EatenSound != null)
					{
						simulation.soundEffectManager.PlaySound(simulation.resourceManager.Resources[num].EatenSound);
					}
					else
					{
						simulation.soundEffectManager.PlaySound("FeedUnit");
					}
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
					entity.WishExpiresAt = null;
					entity.FullnessLength = (ulong)((long)simulation.resourceManager.Resources[num].FullnessTime);
					entity.FullnessRushCostFull = ResourceManager.CalculateFullnessRushCost(entity.FullnessLength);
					if (!flag)
					{
						Simulated.Resident.StartHungerTimer(simulated.GetEntity<ResidentEntity>(), simulation);
					}
				}
				else
				{
					TFUtils.DebugLog("Simulated(" + simulated.Id.Describe() + ") could not eat!");
					simulation.soundEffectManager.PlaySound("DontHaveAny");
					simulated.Variable["RequestOpenInventory"] = true;
					if (activeTasksForSimulated.Count > 0)
					{
						simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id), 0UL);
						return;
					}
					simulation.Router.Send(WishCommand.Create(simulated.Id, simulated.Id), 0UL);
				}
				simulated.timebarMixinArgs.hasTimebar = false;
			}

			// Token: 0x06001698 RID: 5784 RVA: 0x00097B60 File Offset: 0x00095D60
			public void Leave(Simulation simulation, Simulated simulated)
			{
				if (simulated.Variable.ContainsKey("RequestOpenInventory"))
				{
					simulated.Variable.Remove("RequestOpenInventory");
				}
			}

			// Token: 0x06001699 RID: 5785 RVA: 0x00097B94 File Offset: 0x00095D94
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return false;
				}
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				int value = entity.HungerResourceId.Value;
				object obj = null;
				if (simulated.Variable.TryGetValue("RequestOpenInventory", out obj) && (bool)obj)
				{
					session.AddAsyncResponse("ShowInventoryHudWidget", true);
					session.AddAsyncResponse("PulseResourceError", value);
					simulated.Variable["RequestOpenInventory"] = false;
				}
				return false;
			}

			// Token: 0x0600169A RID: 5786 RVA: 0x00097C64 File Offset: 0x00095E64
			private void GenerateAndRecordBonusEarned(uint fedProductId, Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Reward reward = null;
				if (entity.ForcedBonusReward != null)
				{
					reward = entity.ForcedBonusReward.GenerateReward(simulation, true);
					entity.ForcedBonusReward = null;
				}
				else
				{
					int num = entity.BonusPaytables.Length;
					for (int i = 0; i < num; i++)
					{
						if (i == 0)
						{
							reward = entity.BonusPaytables[i].Spin(fedProductId, simulation, Paytable.CONSOLATION_REWARD);
						}
						else
						{
							reward = entity.BonusPaytables[i].Spin(fedProductId, simulation, Paytable.CONSOLATION_REWARD) + reward;
						}
					}
					if (!simulation.featureManager.CheckFeature("recipe_drops") && reward.RecipeUnlocks != null && reward.RecipeUnlocks.Count > 0)
					{
						reward = new Reward(new Dictionary<int, int>
						{
							{
								ResourceManager.SOFT_CURRENCY,
								5
							}
						}, null, null, null, null, null, null, null, false, simulation.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].GetResourceTexture());
					}
					if (reward.RecipeUnlocks != null && reward.RecipeUnlocks.Count > 0)
					{
						foreach (int recipeId in reward.RecipeUnlocks)
						{
							simulation.craftManager.ReserveRecipe(recipeId);
						}
					}
				}
				TFUtils.Assert(reward != null, "Got a null match bonus! Need to adjust the bonus paytables so they always get something!");
				entity.MatchBonus = reward;
				if (reward != null)
				{
					simulation.ModifyGameStateSimulated(simulated, new EarnMatchBonusAction(entity.Id, reward));
				}
			}

			// Token: 0x04000F4D RID: 3917
			private const string REQUEST_ERROR_PULSE = "RequestOpenInventory";
		}

		// Token: 0x020002ED RID: 749
		public class WaitingForDeliveryState : Simulated.StateActionDefault
		{
			// Token: 0x0600169C RID: 5788 RVA: 0x00097E20 File Offset: 0x00096020
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}

			// Token: 0x0600169D RID: 5789 RVA: 0x00097E30 File Offset: 0x00096030
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.command = null;
				int value = simulated.GetEntity<ResidentEntity>().HungerResourceId.Value;
				if (simulation.resourceManager.HasEnough(value, 1))
				{
					session.AddAsyncResponse("GoodDeliveryRequest", new GoodToSimulatedDeliveryRequest(simulated, value, simulation.resourceManager.Resources[value].GetResourceTexture()));
				}
				simulation.Router.Send(OfferFoodCommand.Create(simulated.Id, simulated.Id, value));
				return false;
			}
		}

		// Token: 0x020002EE RID: 750
		public abstract class TransitionallyAnimating : Simulated.StateActionDefault
		{
			// Token: 0x0600169F RID: 5791 RVA: 0x00097EB8 File Offset: 0x000960B8
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState(this.DisplayStateName);
				string displayThoughtMaterial = this.GetDisplayThoughtMaterial(simulation, simulated);
				if (displayThoughtMaterial != null)
				{
					simulated.DisplayThoughtState(displayThoughtMaterial, this.DisplayThoughtStateName, simulation);
				}
				else
				{
					simulated.DisplayThoughtState(this.DisplayThoughtStateName, simulation);
				}
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), (ulong)((long)this.AnimationLength));
			}

			// Token: 0x060016A0 RID: 5792 RVA: 0x00097F48 File Offset: 0x00096148
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				simulated.DisplayThoughtState(null, simulation);
			}

			// Token: 0x17000308 RID: 776
			// (get) Token: 0x060016A1 RID: 5793
			protected abstract string DisplayStateName { get; }

			// Token: 0x17000309 RID: 777
			// (get) Token: 0x060016A2 RID: 5794
			protected abstract string DisplayThoughtStateName { get; }

			// Token: 0x060016A3 RID: 5795
			protected abstract string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated);

			// Token: 0x1700030A RID: 778
			// (get) Token: 0x060016A4 RID: 5796
			protected abstract int AnimationLength { get; }
		}

		// Token: 0x020002EF RID: 751
		public class CheeringState : Simulated.Resident.TransitionallyAnimating
		{
			// Token: 0x1700030B RID: 779
			// (get) Token: 0x060016A6 RID: 5798 RVA: 0x00097F68 File Offset: 0x00096168
			protected override string DisplayStateName
			{
				get
				{
					return "cheer";
				}
			}

			// Token: 0x1700030C RID: 780
			// (get) Token: 0x060016A7 RID: 5799 RVA: 0x00097F70 File Offset: 0x00096170
			protected override string DisplayThoughtStateName
			{
				get
				{
					return null;
				}
			}

			// Token: 0x060016A8 RID: 5800 RVA: 0x00097F74 File Offset: 0x00096174
			protected override string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated)
			{
				return null;
			}

			// Token: 0x1700030D RID: 781
			// (get) Token: 0x060016A9 RID: 5801 RVA: 0x00097F78 File Offset: 0x00096178
			protected override int AnimationLength
			{
				get
				{
					return 2;
				}
			}
		}

		// Token: 0x020002F0 RID: 752
		public class EatingState : Simulated.Resident.TransitionallyAnimating
		{
			// Token: 0x1700030E RID: 782
			// (get) Token: 0x060016AB RID: 5803 RVA: 0x00097F84 File Offset: 0x00096184
			protected override string DisplayStateName
			{
				get
				{
					return "eat";
				}
			}

			// Token: 0x1700030F RID: 783
			// (get) Token: 0x060016AC RID: 5804 RVA: 0x00097F8C File Offset: 0x0009618C
			protected override string DisplayThoughtStateName
			{
				get
				{
					return "hungry";
				}
			}

			// Token: 0x060016AD RID: 5805 RVA: 0x00097F94 File Offset: 0x00096194
			protected override string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated)
			{
				return simulation.resourceManager.Resources[simulated.GetEntity<ResidentEntity>().HungerResourceId.Value].GetResourceTexture();
			}

			// Token: 0x17000310 RID: 784
			// (get) Token: 0x060016AE RID: 5806 RVA: 0x00097FCC File Offset: 0x000961CC
			protected override int AnimationLength
			{
				get
				{
					return 3;
				}
			}

			// Token: 0x060016AF RID: 5807 RVA: 0x00097FD0 File Offset: 0x000961D0
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				this.SpawnAndRecordRewards(simulation, simulated);
				TFUtils.Assert(simulated.ThoughtDisplayController != null, "This simulated doesn't have a thought mask display controller assigned! simulated=" + simulated);
				if (simulated.ThoughtMaskDisplayController != null)
				{
					simulated.ThoughtMaskDisplayController.DisplayState("consume");
				}
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Food_Crumbs", 0, 0, 0f, simulated.eatParticleRequestDelegate);
			}

			// Token: 0x060016B0 RID: 5808 RVA: 0x0009804C File Offset: 0x0009624C
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				simulation.soundEffectManager.PlaySound("EatComplete");
				simulated.GetEntity<ResidentEntity>().HungerResourceId = null;
			}

			// Token: 0x060016B1 RID: 5809 RVA: 0x00098088 File Offset: 0x00096288
			private void SpawnAndRecordRewards(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				TFUtils.Assert(entity.HungerResourceId != null, "Trying to spawn rewards but resident has not HungerResourceID. Did you clear it too early?");
				int value = entity.HungerResourceId.Value;
				simulation.resourceManager.Spend(value, 1, simulation.game);
				bool forceNoWishPayout = simulation.resourceManager.Resources[value].ForceNoWishPayout;
				if (forceNoWishPayout)
				{
					simulation.game.analytics.LogCharacterFeed(entity.DefinitionId, value);
					AnalyticsWrapper.LogCharacterFeed(simulation.game, entity, value, null);
					FeedUnitAction feedUnitAction = new FeedUnitAction(simulated, (ulong)((long)simulation.resourceManager.Resources[value].FullnessTime), value, entity.PreviousResourceId, null);
					feedUnitAction.AddDropData(simulated, null);
					simulation.ModifyGameStateSimulated(simulated, feedUnitAction);
					return;
				}
				Reward reward = simulation.resourceManager.Resources[value].Reward.GenerateReward(simulation, false);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow, false);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Resident.EatingState.SpawnAndRecordRewards - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
				simulation.game.analytics.LogCharacterFeed(entity.DefinitionId, value);
				AnalyticsWrapper.LogCharacterFeed(simulation.game, entity, value, reward);
				FeedUnitAction feedUnitAction2 = new FeedUnitAction(simulated, (ulong)((long)simulation.resourceManager.Resources[value].FullnessTime), value, entity.PreviousResourceId, reward);
				feedUnitAction2.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, feedUnitAction2);
			}
		}

		// Token: 0x020002F1 RID: 753
		public class TryBonusSpinState : Simulated.StateActionDefault
		{
			// Token: 0x060016B3 RID: 5811 RVA: 0x00098238 File Offset: 0x00096438
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.command = null;
				entity.HomeAvailability = false;
				if (entity.MatchBonus != null)
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), 0UL);
				}
				else
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id), 0UL);
				}
			}
		}

		// Token: 0x020002F2 RID: 754
		public class WaitingForCollectBonusState : Simulated.StateActionDefault
		{
			// Token: 0x060016B5 RID: 5813 RVA: 0x000982B0 File Offset: 0x000964B0
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				TFUtils.Assert(simulated.GetEntity<ResidentEntity>().MatchBonus != null, "Got into WaitingForCollectBonus but there is no earned bonus!");
				simulated.DisplayState("acceptable");
				simulated.DisplayThoughtState("bonus_ready", simulation);
				simulation.soundEffectManager.PlaySound("MatchBonus_Ready");
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}

			// Token: 0x060016B6 RID: 5814 RVA: 0x0009832C File Offset: 0x0009652C
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.soundEffectManager.PlaySound("MatchBonus_Open");
				this.CollectAndRecordRewards(simulation, simulated);
				Simulated.Resident.StartHungerTimer(simulated.GetEntity<ResidentEntity>(), simulation);
			}

			// Token: 0x060016B7 RID: 5815 RVA: 0x00098360 File Offset: 0x00096560
			private void CollectAndRecordRewards(Simulation simulation, Simulated simulated)
			{
				Simulated.Resident.RefreshModifiedDisplayState(simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Reward matchBonus = entity.MatchBonus;
				entity.MatchBonus = null;
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(matchBonus, simulation, simulated, TFUtils.EpochTime(), false);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Resident.WaitingForCollectBonusState.CollectAndRecordRewards - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
				CollectMatchBonusAction collectMatchBonusAction = new CollectMatchBonusAction(simulated.Id, matchBonus);
				collectMatchBonusAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, collectMatchBonusAction);
				collectMatchBonusAction.AddPickup(simulation);
				AnalyticsWrapper.LogBonusChest(simulation.game, simulated, matchBonus);
				if (ResourceManager.SPONGY_GAMES_CURRENCY >= 0)
				{
					int b = 0;
					if (matchBonus.ResourceAmounts.TryGetValue(ResourceManager.SPONGY_GAMES_CURRENCY, out b))
					{
						SBMISoaring.AddFoodToCharacter(b, simulated.GetEntity<ResidentEntity>().DefinitionId, -1, null);
					}
				}
			}
		}

		// Token: 0x020002F3 RID: 755
		public class CheeringAfterBonusState : Simulated.Resident.CheeringState
		{
			// Token: 0x17000311 RID: 785
			// (get) Token: 0x060016B9 RID: 5817 RVA: 0x00098458 File Offset: 0x00096658
			protected override string DisplayThoughtStateName
			{
				get
				{
					return "bonus_collect";
				}
			}
		}

		// Token: 0x020002F4 RID: 756
		public class StartingWanderCycleState : Simulated.StateActionDefault
		{
			// Token: 0x060016BB RID: 5819 RVA: 0x00098468 File Offset: 0x00096668
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.Clear();
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.Variable["speed"] = simulated.Invariable["base_speed"];
				simulation.Router.Send(WanderCommand.Create(simulated.Id, simulated.Id));
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}
		}

		// Token: 0x020002F5 RID: 757
		public class RequestingInterfaceState : Simulated.StateAction
		{
			// Token: 0x060016BD RID: 5821 RVA: 0x00098538 File Offset: 0x00096738
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				this.bComplete = false;
			}

			// Token: 0x060016BE RID: 5822 RVA: 0x00098548 File Offset: 0x00096748
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (this.bComplete)
				{
					session.CheckAsyncRequest("RequestEntityInterface");
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
					this.bComplete = false;
					return false;
				}
				if (simulation.game.taskManager.GetTaskingStateForSimulated(simulation, simulated.entity.DefinitionId, simulated.Id) == TaskManager._eBlueprintTaskingState.eNone)
				{
					simulated.InteractionState.SelectedTransition = new Session.UnitIdleTransition(simulated);
				}
				else
				{
					simulated.InteractionState.SelectedTransition = new Session.UnitBusyTransition(simulated);
				}
				session.AddAsyncResponse("RequestEntityInterface", simulated, false);
				this.bComplete = true;
				return false;
			}

			// Token: 0x060016BF RID: 5823 RVA: 0x000985F8 File Offset: 0x000967F8
			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x04000F4E RID: 3918
			private bool bComplete;
		}

		// Token: 0x020002F6 RID: 758
		public class ReflectingState : Simulated.StateAction
		{
			// Token: 0x060016C1 RID: 5825 RVA: 0x00098604 File Offset: 0x00096804
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.Variable["speed"] = simulated.Invariable["base_speed"];
			}

			// Token: 0x060016C2 RID: 5826 RVA: 0x00098638 File Offset: 0x00096838
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return false;
				}
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Command command;
				if (entity.HungerResourceId != null)
				{
					command = ResumeWishingCommand.Create(simulated.Id, simulated.Id);
				}
				else
				{
					command = ResumeFullCommand.Create(simulated.Id, simulated.Id);
				}
				simulation.Router.Send(command);
				return false;
			}

			// Token: 0x060016C3 RID: 5827 RVA: 0x000986E4 File Offset: 0x000968E4
			public void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		// Token: 0x020002F7 RID: 759
		public class TaskDelegatingState : Simulated.StateAction
		{
			// Token: 0x060016C5 RID: 5829 RVA: 0x000986F0 File Offset: 0x000968F0
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				Task task = null;
				if (activeTasksForSimulated.Count > 0)
				{
					task = activeTasksForSimulated[0];
				}
				if (task == null)
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				TaskData._eTaskType eTaskType = task.m_pTaskData.m_eTaskType;
				if (eTaskType == TaskData._eTaskType.eEnter && simulated.GetEntity<ResidentEntity>().MatchBonus != null)
				{
					simulation.Router.Send(EnterCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				if (task.GetTimeLeft() <= 0UL)
				{
					simulation.game.triggerRouter.RouteTrigger(ReevaluateTrigger.CreateTrigger(), simulation.game);
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				if (eTaskType == TaskData._eTaskType.eEnter || eTaskType == TaskData._eTaskType.eActivate || eTaskType == TaskData._eTaskType.eStand)
				{
					Simulated simulated2 = simulation.FindSimulated(task.m_pTargetIdentity);
					simulation.Router.Send(MoveCommand.Create(simulated.Id, simulated.Id, simulated2.PointOfInterest + task.m_pTaskData.m_pPosOffsetFromTarget, simulated2.Flip));
					return;
				}
				simulation.Router.Send(WanderCommand.Create(simulated.Id, simulated.Id));
			}

			// Token: 0x060016C6 RID: 5830 RVA: 0x0009885C File Offset: 0x00096A5C
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x060016C7 RID: 5831 RVA: 0x00098860 File Offset: 0x00096A60
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		// Token: 0x020002F8 RID: 760
		public class TaskUpdateState : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x060016C9 RID: 5833 RVA: 0x0009886C File Offset: 0x00096A6C
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
				if (activeTasksForSimulated.Count > 0)
				{
					entity.m_pTask = activeTasksForSimulated[0];
				}
				simulated.command = null;
				if (entity.HungerResourceId != null)
				{
					Resource resource = simulation.resourceManager.Resources[entity.HungerResourceId.Value];
					string resourceTexture = resource.GetResourceTexture();
					simulated.DisplayThoughtState(resourceTexture, "hungry", simulation);
				}
				else
				{
					simulated.DisplayThoughtState(null, simulation);
				}
				simulated.InteractionState.SetInteractions(false, false, false, true, simulated.InteractionState.SelectedTransition, null);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.SimulatedQueryable = true;
			}

			// Token: 0x060016CA RID: 5834 RVA: 0x00098948 File Offset: 0x00096B48
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x060016CB RID: 5835 RVA: 0x0009894C File Offset: 0x00096B4C
			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.m_pTask.GetTimeLeft() <= 0UL)
				{
					simulation.game.triggerRouter.RouteTrigger(ReevaluateTrigger.CreateTrigger(), simulation.game);
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				}
				ulong num = TFUtils.EpochTime();
				int? num2 = null;
				if (entity.HungerResourceId != null)
				{
					ulong? wishExpiresAt = entity.WishExpiresAt;
					if (wishExpiresAt != null && wishExpiresAt.Value <= num)
					{
						num2 = Simulated.Resident.GenerateHungerResourceID(simulation, entity);
						if (num2 != null)
						{
							entity.PreviousResourceId = entity.HungerResourceId;
						}
					}
				}
				else if (entity.HungryAt <= num)
				{
					num2 = Simulated.Resident.GenerateHungerResourceID(simulation, entity);
				}
				if (num2 != null && num2 == ResourceManager.DEFAULT_WISH)
				{
					num2 = null;
				}
				if (num2 != null)
				{
					entity.HungerResourceId = num2;
					entity.WishExpiresAt = new ulong?(num + (ulong)((long)entity.WishDuration));
					simulation.ModifyGameStateSimulated(simulated, new NewWishAction(simulated.Id, entity.HungerResourceId.Value, entity.PreviousResourceId, entity.WishExpiresAt.Value));
					this.ShowNewHungerResource(simulation, simulated, entity.HungerResourceId.Value);
				}
				return false;
			}

			// Token: 0x060016CC RID: 5836 RVA: 0x00098AE0 File Offset: 0x00096CE0
			protected virtual void ShowNewHungerResource(Simulation simulation, Simulated simulated, int nDID)
			{
				Resource resource = simulation.resourceManager.Resources[nDID];
				string resourceTexture = resource.GetResourceTexture();
				simulated.DisplayThoughtState(resourceTexture, "hungry", simulation);
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Thought_Pop", 0, 0, 0f, simulated.thoughtBubblePopParticleRequestDelegate);
			}
		}

		// Token: 0x020002F9 RID: 761
		public class TaskIdleState : Simulated.Resident.TaskUpdateState
		{
			// Token: 0x060016CE RID: 5838 RVA: 0x00098B3C File Offset: 0x00096D3C
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				float fWanderTime = entity.m_pTask.m_pTaskData.m_fWanderTime;
				float fIdleTime = entity.m_pTask.m_pTaskData.m_fIdleTime;
				if (fIdleTime <= 0f && fWanderTime > 0f)
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				simulated.DisplayState(entity.m_pTask.m_pTaskData.m_sSourceDisplayStateIdle);
				entity.StartCheckForResume(Mathf.RoundToInt(fIdleTime), Mathf.RoundToInt(fIdleTime));
			}

			// Token: 0x060016CF RID: 5839 RVA: 0x00098BD8 File Offset: 0x00096DD8
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				simulated.GetEntity<ResidentEntity>().StopCheckForResume();
			}

			// Token: 0x060016D0 RID: 5840 RVA: 0x00098BF0 File Offset: 0x00096DF0
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.Simulate(simulation, simulated, session);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				float fWanderTime = entity.m_pTask.m_pTaskData.m_fWanderTime;
				if (fWanderTime > 0f && entity.CheckForResume())
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}
		}

		// Token: 0x020002FA RID: 762
		public class TaskWanderState : Simulated.Resident.TaskUpdateState
		{
			// Token: 0x060016D2 RID: 5842 RVA: 0x00098C5C File Offset: 0x00096E5C
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				float fWanderTime = entity.m_pTask.m_pTaskData.m_fWanderTime;
				float fIdleTime = entity.m_pTask.m_pTaskData.m_fIdleTime;
				if (fWanderTime <= 0f && fIdleTime > 0f)
				{
					simulation.Router.Send(IdlePauseCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				float fMovementSpeed = entity.m_pTask.m_pTaskData.m_fMovementSpeed;
				if (fMovementSpeed > 0f)
				{
					simulated.Variable["speed"] = fMovementSpeed;
				}
				simulated.DisplayState(entity.m_pTask.m_pTaskData.m_sSourceDisplayStateWalk);
				entity.StartCheckForIdle(Mathf.RoundToInt(fWanderTime), Mathf.RoundToInt(fWanderTime));
			}

			// Token: 0x060016D3 RID: 5843 RVA: 0x00098D28 File Offset: 0x00096F28
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				simulated.Warp(simulated.Position, null);
				simulated.GetEntity<ResidentEntity>().StopCheckForIdle();
			}

			// Token: 0x060016D4 RID: 5844 RVA: 0x00098D58 File Offset: 0x00096F58
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.Simulate(simulation, simulated, session);
				base.RandomWanderSimulate(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				float fIdleTime = entity.m_pTask.m_pTaskData.m_fIdleTime;
				if (fIdleTime > 0f && entity.CheckForIdle())
				{
					simulated.Variable["pathing"] = null;
					simulated.ClearPathInfo();
					simulated.Warp(simulated.Position, null);
					simulation.Router.Send(IdlePauseCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}
		}

		// Token: 0x020002FB RID: 763
		public class TaskMovingState : Simulated.Resident.TaskUpdateState
		{
			// Token: 0x060016D6 RID: 5846 RVA: 0x00098DF0 File Offset: 0x00096FF0
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				Command command = simulated.command;
				base.Enter(simulation, simulated);
				simulated.command = command;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.m_bReachedTaskTarget = false;
				if (!entity.m_pTask.m_bMovingToTarget)
				{
					entity.m_pTask.m_bMovingToTarget = true;
					entity.m_pTask.m_ulMovingTimeStart = TFUtils.EpochTime();
				}
				float fMovementSpeed = entity.m_pTask.m_pTaskData.m_fMovementSpeed;
				TFUtils.ErrorLog("simulated's current speed: " + fMovementSpeed);
				if (fMovementSpeed > 0f)
				{
					simulated.Variable["speed"] = fMovementSpeed;
				}
				entity.m_pTaskTargetPosition = (Vector2)simulated.command["position"];
				if (simulated.Position == entity.m_pTaskTargetPosition)
				{
					simulated.command = null;
					this.ReachedTarget(simulation, simulated);
					return;
				}
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, entity.m_pTaskTargetPosition);
				simulated.ClearPathInfo();
				simulated.DisplayState(entity.m_pTask.m_pTaskData.m_sSourceDisplayStateWalk);
				simulated.command = null;
			}

			// Token: 0x060016D7 RID: 5847 RVA: 0x00098F1C File Offset: 0x0009711C
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.m_bReachedTaskTarget)
				{
					return false;
				}
				base.Simulate(simulation, simulated, session);
				Simulated simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
				if (simulated2 != null && !(session.TheState is Session.MoveBuilding) && entity.m_pTaskTargetPosition != simulated2.PointOfInterest + entity.m_pTask.m_pTaskData.m_pPosOffsetFromTarget)
				{
					entity.m_pTaskTargetPosition = simulated2.PointOfInterest + entity.m_pTask.m_pTaskData.m_pPosOffsetFromTarget;
					simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, entity.m_pTaskTargetPosition);
					simulated.ClearPathInfo();
				}
				if (base.FollowPathSimulate(simulation, simulated))
				{
					this.ReachedTarget(simulation, simulated);
				}
				return false;
			}

			// Token: 0x060016D8 RID: 5848 RVA: 0x00098FFC File Offset: 0x000971FC
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				simulated.Warp(simulated.Position, null);
				simulated.ClearPathInfo();
			}

			// Token: 0x060016D9 RID: 5849 RVA: 0x0009901C File Offset: 0x0009721C
			private void ReachedTarget(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.m_bReachedTaskTarget = true;
				simulated.Position = entity.m_pTaskTargetPosition;
				simulated.Warp(simulated.Position, null);
				TaskData._eTaskType eTaskType = entity.m_pTask.m_pTaskData.m_eTaskType;
				if (eTaskType == TaskData._eTaskType.eEnter)
				{
					simulated.DisplayThoughtState(null, simulation);
					simulated.Visible = false;
					simulation.Router.Send(EnterCommand.Create(simulated.Id, simulated.Id));
				}
				else
				{
					simulation.Router.Send(StandCommand.Create(simulated.Id, simulated.Id));
				}
			}
		}

		// Token: 0x020002FC RID: 764
		public class TaskEnterState : Simulated.Resident.TaskUpdateState
		{
			// Token: 0x060016DB RID: 5851 RVA: 0x000990BC File Offset: 0x000972BC
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.InteractionState.Clear();
				simulated.Visible = false;
				simulated.Variable["pathing"] = null;
				simulated.DisplayThoughtState(null, simulation);
				simulated.ClearPathInfo();
				simulated.Position = new Vector3(9999999f, 9999999f, 9999999f);
				simulated.Warp(simulated.Position, null);
				entity.m_pTask.m_bAtTarget = true;
				if (entity.m_pTask.m_bMovingToTarget)
				{
					entity.m_pTask.m_bMovingToTarget = false;
					ulong num = TFUtils.EpochTime() - entity.m_pTask.m_ulMovingTimeStart;
					entity.m_pTask.m_ulMovingTimeStart = 0UL;
					entity.m_pTask.m_ulCompleteTime += num;
					entity.m_pTask.m_ulStartTime += num;
					simulation.ModifyGameState(new TaskUpdateAction(entity.m_pTask));
				}
			}

			// Token: 0x060016DC RID: 5852 RVA: 0x000991B8 File Offset: 0x000973B8
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.Simulate(simulation, simulated, session);
				return false;
			}

			// Token: 0x060016DD RID: 5853 RVA: 0x000991C8 File Offset: 0x000973C8
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Simulated simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
				if (simulated2 == null)
				{
					simulated2 = simulation.FindSimulated(entity.Residence);
				}
				simulated.Position = simulated2.PointOfInterest + entity.m_pTask.m_pTaskData.m_pPosOffsetFromTarget;
				simulated.Warp(simulated.Position, null);
			}

			// Token: 0x060016DE RID: 5854 RVA: 0x00099238 File Offset: 0x00097438
			protected override void ShowNewHungerResource(Simulation simulation, Simulated simulated, int nDID)
			{
			}
		}

		// Token: 0x020002FD RID: 765
		public class TaskEnterFeedState : Simulated.Resident.TaskUpdateState
		{
			// Token: 0x060016E0 RID: 5856 RVA: 0x00099244 File Offset: 0x00097444
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.InteractionState.Clear();
				simulated.Visible = false;
				simulated.Variable["pathing"] = null;
				simulated.DisplayThoughtState(null, simulation);
				simulated.ClearPathInfo();
				simulated.Position = new Vector3(9999999f, 9999999f, 9999999f);
				simulated.Warp(simulated.Position, null);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.m_pTask.m_bAtTarget = true;
				if (entity.m_pTask.m_bMovingToTarget)
				{
					entity.m_pTask.m_bMovingToTarget = false;
					ulong num = TFUtils.EpochTime() - entity.m_pTask.m_ulMovingTimeStart;
					entity.m_pTask.m_ulMovingTimeStart = 0UL;
					entity.m_pTask.m_ulCompleteTime += num;
					entity.m_pTask.m_ulStartTime += num;
					simulation.ModifyGameState(new TaskUpdateAction(entity.m_pTask));
				}
				if (entity.HungerResourceId != null)
				{
					int value = entity.HungerResourceId.Value;
					if (simulation.resourceManager.HasEnough(value, 1))
					{
						this.GenerateAndRecordBonusEarned((uint)value, simulation, simulated);
						if (simulation.resourceManager.Resources[value].EatenSound != null)
						{
							simulation.soundEffectManager.PlaySound(simulation.resourceManager.Resources[value].EatenSound);
						}
						else
						{
							simulation.soundEffectManager.PlaySound("FeedUnit");
						}
						entity.WishExpiresAt = null;
						entity.FullnessLength = (ulong)((long)simulation.resourceManager.Resources[value].FullnessTime);
						entity.FullnessRushCostFull = ResourceManager.CalculateFullnessRushCost(entity.FullnessLength);
						this.SpawnAndRecordRewards(simulation, simulated);
						int value2 = entity.HungerResourceId.Value;
						entity.PreviousResourceId = new int?(value2);
						entity.HungerResourceId = null;
						entity.WishExpiresAt = null;
						entity.HungryAt = TFUtils.EpochTime() + entity.FullnessLength;
						Simulated simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
						simulation.Router.Send(BonusRewardCommand.Create(simulated.Id, simulated2.Id, simulated.entity.DefinitionId));
					}
					else
					{
						TFUtils.DebugLog("Simulated(" + simulated.Id.Describe() + ") could not eat!");
						simulation.soundEffectManager.PlaySound("DontHaveAny");
						simulated.Variable["RequestOpenInventory"] = true;
						simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					}
				}
			}

			// Token: 0x060016E1 RID: 5857 RVA: 0x00099508 File Offset: 0x00097708
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.Simulate(simulation, simulated, session);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.MatchBonus == null)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}

			// Token: 0x060016E2 RID: 5858 RVA: 0x00099550 File Offset: 0x00097750
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.Position = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity).PointOfInterest + entity.m_pTask.m_pTaskData.m_pPosOffsetFromTarget;
				simulated.Warp(simulated.Position, null);
			}

			// Token: 0x060016E3 RID: 5859 RVA: 0x000995AC File Offset: 0x000977AC
			protected override void ShowNewHungerResource(Simulation simulation, Simulated simulated, int nDID)
			{
			}

			// Token: 0x060016E4 RID: 5860 RVA: 0x000995B0 File Offset: 0x000977B0
			private void GenerateAndRecordBonusEarned(uint fedProductId, Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Reward reward = null;
				if (entity.ForcedBonusReward != null)
				{
					reward = entity.ForcedBonusReward.GenerateReward(simulation, true);
					entity.ForcedBonusReward = null;
				}
				else
				{
					int num = entity.BonusPaytables.Length;
					for (int i = 0; i < num; i++)
					{
						if (i == 0)
						{
							reward = entity.BonusPaytables[i].Spin(fedProductId, simulation, Paytable.CONSOLATION_REWARD);
						}
						else
						{
							reward = entity.BonusPaytables[i].Spin(fedProductId, simulation, Paytable.CONSOLATION_REWARD) + reward;
						}
					}
					if (!simulation.featureManager.CheckFeature("recipe_drops") && reward.RecipeUnlocks != null && reward.RecipeUnlocks.Count > 0)
					{
						reward = new Reward(new Dictionary<int, int>
						{
							{
								ResourceManager.SOFT_CURRENCY,
								5
							}
						}, null, null, null, null, null, null, null, false, simulation.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].GetResourceTexture());
					}
					if (reward.RecipeUnlocks != null && reward.RecipeUnlocks.Count > 0)
					{
						foreach (int recipeId in reward.RecipeUnlocks)
						{
							simulation.craftManager.ReserveRecipe(recipeId);
						}
					}
				}
				TFUtils.Assert(reward != null, "Got a null match bonus! Need to adjust the bonus paytables so they always get something!");
				entity.MatchBonus = reward;
				if (reward != null)
				{
					simulation.ModifyGameStateSimulated(simulated, new EarnMatchBonusAction(entity.Id, reward));
				}
			}

			// Token: 0x060016E5 RID: 5861 RVA: 0x00099764 File Offset: 0x00097964
			private void SpawnAndRecordRewards(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				TFUtils.Assert(entity.HungerResourceId != null, "Trying to spawn rewards but resident has not HungerResourceID. Did you clear it too early?");
				int value = entity.HungerResourceId.Value;
				simulation.resourceManager.Spend(value, 1, simulation.game);
				bool forceNoWishPayout = simulation.resourceManager.Resources[value].ForceNoWishPayout;
				if (forceNoWishPayout)
				{
					simulation.game.analytics.LogCharacterFeed(entity.DefinitionId, value);
					AnalyticsWrapper.LogCharacterFeed(simulation.game, entity, value, null);
					FeedUnitAction feedUnitAction = new FeedUnitAction(simulated, (ulong)((long)simulation.resourceManager.Resources[value].FullnessTime), value, entity.PreviousResourceId, null);
					feedUnitAction.AddDropData(simulated, null);
					simulation.ModifyGameStateSimulated(simulated, feedUnitAction);
					return;
				}
				Simulated simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
				Reward reward = simulation.resourceManager.Resources[value].Reward.GenerateReward(simulation, false);
				ulong utcNow = TFUtils.EpochTime();
				simulated2.DisplayThoughtState("bonus_ready", simulation);
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated2, utcNow, false);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Resident.EatingState.SpawnAndRecordRewards - dropResults is null");
					return;
				}
				simulated2.DisplayThoughtState(null, simulation);
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
				simulation.game.analytics.LogCharacterFeed(entity.DefinitionId, value);
				AnalyticsWrapper.LogCharacterFeed(simulation.game, entity, value, reward);
				FeedUnitAction feedUnitAction2 = new FeedUnitAction(simulated, (ulong)((long)simulation.resourceManager.Resources[value].FullnessTime), value, entity.PreviousResourceId, reward);
				feedUnitAction2.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, feedUnitAction2);
			}

			// Token: 0x04000F4F RID: 3919
			private const string REQUEST_ERROR_PULSE = "RequestOpenInventory";
		}

		// Token: 0x020002FE RID: 766
		public class TaskStandState : Simulated.Resident.TaskUpdateState
		{
			// Token: 0x060016E7 RID: 5863 RVA: 0x00099940 File Offset: 0x00097B40
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.Variable["pathing"] = null;
				simulated.ClearPathInfo();
				simulated.DisplayState(entity.m_pTask.m_pTaskData.m_sSourceDisplayStateIdle);
				if (!entity.m_pTask.m_bAtTarget)
				{
					entity.m_pTask.m_bAtTarget = true;
					if (entity.m_pTask.m_pTaskData.m_eTaskType == TaskData._eTaskType.eActivate)
					{
						Simulated simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
						if (simulated2 != null)
						{
							entity.m_pTask.m_sTargetPrevDisplayState = simulated2.GetDisplayState();
							simulated2.DisplayState(entity.m_pTask.m_pTaskData.m_sTargetDisplayState);
						}
					}
				}
				simulated.Position = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity).PointOfInterest + entity.m_pTask.m_pTaskData.m_pPosOffsetFromTarget;
				simulated.Warp(simulated.Position, null);
				if (entity.m_pTask.m_bMovingToTarget)
				{
					entity.m_pTask.m_bMovingToTarget = false;
					ulong num = TFUtils.EpochTime() - entity.m_pTask.m_ulMovingTimeStart;
					entity.m_pTask.m_ulMovingTimeStart = 0UL;
					entity.m_pTask.m_ulCompleteTime += num;
					entity.m_pTask.m_ulStartTime += num;
					simulation.ModifyGameState(new TaskUpdateAction(entity.m_pTask));
				}
			}

			// Token: 0x060016E8 RID: 5864 RVA: 0x00099AB0 File Offset: 0x00097CB0
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.Simulate(simulation, simulated, session);
				return false;
			}

			// Token: 0x060016E9 RID: 5865 RVA: 0x00099AC0 File Offset: 0x00097CC0
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
			}
		}

		// Token: 0x020002FF RID: 767
		public class TaskCollectRewardState : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x060016EB RID: 5867 RVA: 0x00099AD4 File Offset: 0x00097CD4
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.Visible = true;
				int count = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true).Count;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (count != 0)
				{
					Task task = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true)[0];
					int nDID = task.m_pTaskData.m_nDID;
					Simulated simulated2 = null;
					if (task.m_bAtTarget)
					{
						TFUtils.ErrorLog("pTask.m_bAtTarget - line 2867 Simulated.Resident.cs");
						task.m_bAtTarget = false;
						if (entity.m_pTask.m_pTaskData.m_eTaskType == TaskData._eTaskType.eActivate && !string.IsNullOrEmpty(entity.m_pTask.m_sTargetPrevDisplayState))
						{
							simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
							if (simulated2 != null)
							{
								simulated2.DisplayState(entity.m_pTask.m_sTargetPrevDisplayState);
							}
						}
					}
					if (task.m_bMovingToTarget)
					{
						TFUtils.ErrorLog("pTask.m_bMovingToTarget - line 2881 Simulated.Resident.cs");
						task.m_bMovingToTarget = false;
						ulong num = TFUtils.EpochTime() - task.m_ulMovingTimeStart;
						task.m_ulMovingTimeStart = 0UL;
						task.m_ulCompleteTime += num;
						task.m_ulStartTime += num;
						simulation.ModifyGameState(new TaskUpdateAction(task));
					}
					if (simulated2 == null && task.m_pTargetIdentity != null)
					{
						simulated2 = simulation.FindSimulated(task.m_pTargetIdentity);
						if (simulated2 != null)
						{
							ulong ulCompleteTime = task.m_ulCompleteTime;
							task.m_ulCompleteTime = TFUtils.EpochTime();
							simulated2.UpdateControls(simulation);
							task.m_ulCompleteTime = ulCompleteTime;
							TFUtils.ErrorLog("pTargetSim != null - line 2894 Simulated.Resident.cs");
						}
					}
					simulated.Variable["speed"] = simulated.Invariable["base_speed"];
					simulated.DisplayState("walk");
					TFUtils.ErrorLog("simulated speed: " + simulated.Variable["speed"]);
					if (task.m_pTaskData.tasksHasBonus.Contains(nDID))
					{
						if (simulation.game.paytableManager.paytableTaskCheck.Contains(nDID))
						{
							simulated.DisplayThoughtState("TreasureChest_Closed.png", "task_collect", simulation);
						}
					}
					else
					{
						simulated.DisplayThoughtState(task.m_pTaskData.m_pReward.ThoughtIcon, "task_collect", simulation);
					}
					simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
					simulation.soundEffectManager.PlaySound("MatchBonus_Ready");
					simulated.InteractionState.SetInteractions(false, false, true, true, null, null);
				}
			}

			// Token: 0x060016EC RID: 5868 RVA: 0x00099D68 File Offset: 0x00097F68
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
				Task task = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true)[0];
				int nDID = task.m_pTaskData.m_nDID;
				simulation.soundEffectManager.PlaySound("MatchBonus_Open");
				if (task.m_pTaskData.tasksHasBonus.Contains(nDID) && simulation.game.paytableManager.paytableTaskCheck.Contains(nDID))
				{
					this.GenerateAndRecordTaskBonusEarned(nDID, task, simulation, simulated);
				}
				this.CollectAndRecordRewards(simulation, simulated);
			}

			// Token: 0x060016ED RID: 5869 RVA: 0x00099E00 File Offset: 0x00098000
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.RandomWanderSimulate(simulation, simulated);
				return false;
			}

			// Token: 0x060016EE RID: 5870 RVA: 0x00099E0C File Offset: 0x0009800C
			private void CollectAndRecordRewards(Simulation simulation, Simulated simulated)
			{
				TFUtils.ErrorLog("into CollectAndRecordRewards - line 2969 Simulated.Resident.cs");
				Task task = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true)[0];
				int nDID = task.m_pTaskData.m_nDID;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Reward reward = entity.MatchBonus;
				entity.MatchBonus = null;
				RewardManager.RewardDropResults rewardDropResults;
				if (simulated.taskBonusReward != null)
				{
					TFUtils.ErrorLog("simulated.taskBonusReward != null  - line 2985 Simulated.Resident.cs");
					reward = simulated.taskBonusReward;
					rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, TFUtils.EpochTime(), false);
					simulated.taskBonusReward = null;
				}
				else
				{
					reward = task.m_pTaskData.m_pReward;
					rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, TFUtils.EpochTime(), false);
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
				simulation.game.analytics.LogTaskCompleted(nDID, simulation.game.resourceManager.PlayerLevelAmount);
				AnalyticsWrapper.LogTaskCompleted(simulation.game, task);
				simulation.game.taskManager.IncrementTaskCompletionCount(nDID);
				TaskCompleteAction taskCompleteAction = new TaskCompleteAction(simulated.Id, task, reward, simulation.game.taskManager.GetTaskCompletionCount(nDID));
				CollectMatchBonusAction collectMatchBonusAction = new CollectMatchBonusAction(simulated.Id, reward);
				taskCompleteAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, taskCompleteAction);
				taskCompleteAction.AddPickup(simulation);
				collectMatchBonusAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, collectMatchBonusAction);
				collectMatchBonusAction.AddPickup(simulation);
				simulation.soundEffectManager.PlaySound(task.m_pTaskData.m_sFinishVO);
				simulation.soundEffectManager.PlaySound(task.m_pTaskData.m_sFinishSound);
				simulation.game.taskManager.RemoveActiveTask(task.m_pTaskData.m_nDID);
				simulation.UpdateControls();
			}

			// Token: 0x060016EF RID: 5871 RVA: 0x00099FF0 File Offset: 0x000981F0
			private void GenerateAndRecordTaskBonusEarned(int taskDID, Task pTask, Simulation simulation, Simulated simulated)
			{
				TFUtils.ErrorLog("made it into GenerateAndRecordTaskBonusEarned - line 3035 Simulated.Resident.cs");
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Reward reward = null;
				if (entity.ForcedBonusReward == null)
				{
					TFUtils.ErrorLog("entity.ForcedBonusReward = null - line 3049 Simulated.Resident.cs");
					int num = entity.BonusPaytables.Length;
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						if (i == 0)
						{
							reward = entity.BonusPaytables[i].Spin(taskDID, simulation, Paytable.CONSOLATION_REWARD) + pTask.m_pTaskData.m_pReward;
						}
						else if (i == 0 || num2 < 1)
						{
						}
						num2++;
					}
					if (!simulation.featureManager.CheckFeature("recipe_drops"))
					{
					}
					if (reward.RecipeUnlocks != null && reward.RecipeUnlocks.Count > 0)
					{
						TFUtils.ErrorLog("bonus.RecipeUnlocks != null - line 3099 Simulated.Resident.cs");
						foreach (int recipeId in reward.RecipeUnlocks)
						{
							simulation.craftManager.ReserveRecipe(recipeId);
						}
					}
					else if (reward.BuildingUnlocks == null || reward.BuildingUnlocks.Count > 0)
					{
					}
				}
				TFUtils.Assert(reward != null, "Got a null match bonus! Need to adjust the bonus paytables so they always get something!");
				if (reward != null)
				{
					simulation.ModifyGameStateSimulated(simulated, new EarnMatchBonusAction(entity.Id, reward));
					simulated.taskBonusReward = reward;
				}
			}
		}

		// Token: 0x02000300 RID: 768
		public class TaskCheerAfterCollectState : Simulated.Resident.CheeringState
		{
			// Token: 0x17000312 RID: 786
			// (get) Token: 0x060016F1 RID: 5873 RVA: 0x0009A190 File Offset: 0x00098390
			protected override string DisplayThoughtStateName
			{
				get
				{
					return null;
				}
			}
		}
	}

	// Token: 0x02000301 RID: 769
	public class Treasure
	{
		// Token: 0x060016F4 RID: 5876 RVA: 0x0009A1E8 File Offset: 0x000983E8
		public static Simulated Load(TreasureEntity treasureEntity, Simulation simulation, Vector2 position, ulong utcNow)
		{
			string key;
			if (treasureEntity.ClearCompleteTime != null)
			{
				ulong? clearCompleteTime = treasureEntity.ClearCompleteTime;
				if (clearCompleteTime != null && clearCompleteTime.Value <= utcNow)
				{
					key = "claiming";
					goto IL_91;
				}
			}
			if (treasureEntity.ClearCompleteTime != null)
			{
				ulong? clearCompleteTime2 = treasureEntity.ClearCompleteTime;
				if (clearCompleteTime2 != null && clearCompleteTime2.Value > utcNow)
				{
					key = "uncovering";
					goto IL_91;
				}
			}
			key = "buried";
			IL_91:
			Simulated simulated = simulation.CreateSimulated(treasureEntity, EntityManager.TreasureActions[key], position);
			simulated.Warp(position, simulation);
			simulated.Visible = true;
			simulated.SetFootprint(simulation, true);
			return simulated;
		}

		// Token: 0x04000F50 RID: 3920
		public static Simulated.Treasure.BuriedState Buried = new Simulated.Treasure.BuriedState();

		// Token: 0x04000F51 RID: 3921
		public static Simulated.Treasure.UncoveringState Uncovering = new Simulated.Treasure.UncoveringState();

		// Token: 0x04000F52 RID: 3922
		public static Simulated.Treasure.ClaimingState Claiming = new Simulated.Treasure.ClaimingState();

		// Token: 0x04000F53 RID: 3923
		public static Simulated.Treasure.DeletingState Deleting = new Simulated.Treasure.DeletingState();

		// Token: 0x04000F54 RID: 3924
		public static Simulated.Treasure.ClaimingStateFriend Claiming_Friend = new Simulated.Treasure.ClaimingStateFriend();

		// Token: 0x04000F55 RID: 3925
		public static Simulated.Treasure.BuriedStateFriend Buried_Friend = new Simulated.Treasure.BuriedStateFriend();

		// Token: 0x02000302 RID: 770
		public class BuriedState : Simulated.StateAction
		{
			// Token: 0x060016F6 RID: 5878 RVA: 0x0009A2BC File Offset: 0x000984BC
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState("inactive");
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
			}

			// Token: 0x060016F7 RID: 5879 RVA: 0x0009A2FC File Offset: 0x000984FC
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.particleSystemManager.RemoveRequestWithDelegate(simulated.starsParticleSystemRequestDelegate);
				simulated.starsParticleSystemRequestDelegate.isAssigned = false;
				TreasureEntity entity = simulated.GetEntity<TreasureEntity>();
				simulation.ModifyGameStateSimulated(simulated, new TreasureUncoverAction(simulated.Id, TFUtils.EpochTime() + entity.ClearTime));
			}

			// Token: 0x060016F8 RID: 5880 RVA: 0x0009A350 File Offset: 0x00098550
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (!simulated.starsParticleSystemRequestDelegate.isAssigned)
				{
					simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Sparkles_Rising2", 0, 0, 1f, simulated.starsParticleSystemRequestDelegate);
					simulated.starsParticleSystemRequestDelegate.isAssigned = true;
				}
				return false;
			}
		}

		// Token: 0x02000303 RID: 771
		public class UncoveringState : Simulated.StateAction, Simulated.Animated
		{
			// Token: 0x060016FA RID: 5882 RVA: 0x0009A398 File Offset: 0x00098598
			public void Enter(Simulation simulation, Simulated simulated)
			{
				TreasureEntity entity = simulated.GetEntity<TreasureEntity>();
				if (!entity.HasStartedClearing)
				{
					simulation.soundEffectManager.PlaySound("DiggingForTreasure");
					TFUtils.DebugLog(string.Concat(new object[]
					{
						"Treasure(",
						simulated.Id.Describe(),
						"):Uncovering. Ready in ",
						entity.ClearTime
					}));
					entity.ClearCompleteTime = new ulong?(TFUtils.EpochTime() + entity.ClearTime);
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), entity.ClearTime);
				}
				else
				{
					simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id, null);
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), entity.ClearTimeRemaining);
				}
				simulated.command = null;
				entity.RaisingTimeRemaining = entity.ClearTimeRemaining;
				simulated.EnableAnimateAction(true);
			}

			// Token: 0x060016FB RID: 5883 RVA: 0x0009A49C File Offset: 0x0009869C
			public void Leave(Simulation simulation, Simulated simulated)
			{
				TreasureEntity entity = simulated.GetEntity<TreasureEntity>();
				simulated.ClearSimulateOnce();
				entity.RaisingTimeRemaining = 0f;
				BasicSprite basicSprite = (BasicSprite)simulated.DisplayController;
				basicSprite.SetMaskPercentage(0f);
				simulation.particleSystemManager.RemoveRequestWithDelegate(simulated.dustParticleSystemRequestDelegate);
				simulated.dustParticleSystemRequestDelegate.isAssigned = false;
				simulated.ClearSimulateOnce();
				simulated.EnableAnimateAction(false);
				if (entity.Quickclear)
				{
					simulation.Router.Send(ClickedCommand.Create(simulated.Id, simulated.Id));
				}
			}

			// Token: 0x060016FC RID: 5884 RVA: 0x0009A52C File Offset: 0x0009872C
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x060016FD RID: 5885 RVA: 0x0009A530 File Offset: 0x00098730
			public Vector3 Animate(Simulation simulation, Simulated simulated)
			{
				TreasureEntity entity = simulated.GetEntity<TreasureEntity>();
				float num = (entity.ClearTime <= 0UL) ? 0f : (entity.RaisingTimeRemaining / entity.ClearTime);
				entity.RaisingTimeRemaining -= Time.deltaTime;
				num = TFMath.ClampF(num, 0f, 1f);
				if (!simulated.dustParticleSystemRequestDelegate.isAssigned)
				{
					simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Construction_Smoke", 0, 0, 1f, simulated.dustParticleSystemRequestDelegate);
					simulated.dustParticleSystemRequestDelegate.offset = new Vector3(0f, 0f, 10f);
					simulated.dustParticleSystemRequestDelegate.isAssigned = true;
				}
				float d = -num * simulated.Height / 2f;
				simulated.DisplayState("default");
				simulated.DisplayController.SetMaskPercentage(num);
				return simulated.DisplayController.Up * d;
			}
		}

		// Token: 0x02000304 RID: 772
		public class ClaimingState : Simulated.StateAction
		{
			// Token: 0x060016FF RID: 5887 RVA: 0x0009A630 File Offset: 0x00098830
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				simulated.command = null;
				simulated.InteractionState.SetInteractions(false, false, false, true, new Session.ShowTreasureRewardTransition(simulated), null);
				simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
			}

			// Token: 0x06001700 RID: 5888 RVA: 0x0009A674 File Offset: 0x00098874
			public void Leave(Simulation simulation, Simulated simulated)
			{
				this.SpawnDrops(simulation, simulated);
			}

			// Token: 0x06001701 RID: 5889 RVA: 0x0009A680 File Offset: 0x00098880
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x06001702 RID: 5890 RVA: 0x0009A684 File Offset: 0x00098884
			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
				Reward reward = this.GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, new Vector3(simulated.Position.x, simulated.Position.y, 20f), utcNow, false);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Treasure.ClaimingState.SpawnDrops - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = (count <= 0) ? null : rewardDropResults.dropIdentities[count - 1];
				TreasureSpawner treasureTiming = simulated.GetEntity<TreasureEntity>().TreasureTiming;
				treasureTiming.MarkCollected();
				simulation.game.analytics.LogChestPickup(simulated.entity.DefinitionId);
				AnalyticsWrapper.LogChestPickup(simulation.game, simulated, reward);
				TreasureCollectAction treasureCollectAction = new TreasureCollectAction(simulated.Id, reward, treasureTiming.PersistName, treasureTiming.TimeToTreasure);
				treasureCollectAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, treasureCollectAction);
				treasureCollectAction.AddPickup(simulation);
			}

			// Token: 0x06001703 RID: 5891 RVA: 0x0009A784 File Offset: 0x00098984
			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<TreasureEntity>().ClearingReward.GenerateReward(simulation, false);
			}
		}

		// Token: 0x02000305 RID: 773
		public class DeletingState : Simulated.StateActionDefault
		{
			// Token: 0x06001705 RID: 5893 RVA: 0x0009A7A0 File Offset: 0x000989A0
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState(null);
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.Clear();
			}

			// Token: 0x06001706 RID: 5894 RVA: 0x0009A7C4 File Offset: 0x000989C4
			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return true;
			}
		}

		// Token: 0x02000306 RID: 774
		public class ClaimingStateFriend : Simulated.StateAction
		{
			// Token: 0x06001708 RID: 5896 RVA: 0x0009A7D0 File Offset: 0x000989D0
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				simulated.command = null;
				simulated.InteractionState.SetInteractions(false, false, false, true, new Session.ShowTreasureRewardTransition(simulated), null);
				simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
			}

			// Token: 0x06001709 RID: 5897 RVA: 0x0009A814 File Offset: 0x00098A14
			public void Leave(Simulation simulation, Simulated simulated)
			{
				this.SpawnDrops(simulation, simulated);
			}

			// Token: 0x0600170A RID: 5898 RVA: 0x0009A820 File Offset: 0x00098A20
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			// Token: 0x0600170B RID: 5899 RVA: 0x0009A824 File Offset: 0x00098A24
			public int CheckHasValue(SoaringDictionary tobeChecked, string key)
			{
				int result = 0;
				SoaringValue soaringValue = tobeChecked.soaringValue(key);
				if (soaringValue != null)
				{
					result = soaringValue;
				}
				return result;
			}

			// Token: 0x0600170C RID: 5900 RVA: 0x0009A84C File Offset: 0x00098A4C
			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
				Reward reward = this.GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				if (RewardManager.GenerateRewardDrops(reward, simulation, new Vector3(simulated.Position.x, simulated.Position.y, 20f), utcNow, false) == null)
				{
					TFUtils.ErrorLog("Treasure.ClaimingStateFriend.SpawnDrops - dropResults is null");
					return;
				}
				SoaringDictionary soaringDictionary = (SoaringDictionary)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_friends_reward_key");
				int num2;
				foreach (int num in reward.ResourceAmounts.Keys)
				{
					if (num == ResourceManager.SOFT_CURRENCY)
					{
						num2 = this.CheckHasValue(soaringDictionary, "SBMI_friends_coinreward_key");
						num2 += reward.ResourceAmounts[num];
						soaringDictionary.setValue(num2, "SBMI_friends_coinreward_key");
					}
					else if (num == ResourceManager.HARD_CURRENCY)
					{
						num2 = this.CheckHasValue(soaringDictionary, "SBMI_friends_jellyreward_key");
						num2 += reward.ResourceAmounts[num];
						soaringDictionary.setValue(num2, "SBMI_friends_jellyreward_key");
					}
					else if (num == ResourceManager.XP)
					{
						num2 = this.CheckHasValue(soaringDictionary, "SBMI_friends_xpreward_key");
						num2 += reward.ResourceAmounts[num];
						soaringDictionary.setValue(num2, "SBMI_friends_xpreward_key");
					}
				}
				num2 = this.CheckHasValue(soaringDictionary, "SBMI_friends_chestscollected_key");
				num2++;
				soaringDictionary.setValue(num2, "SBMI_friends_chestscollected_key");
				num2 = SBMISoaring.PatchTownTreasureCollected;
				num2++;
				SBMISoaring.PatchTownTreasureCollected = num2;
				Soaring.UpdateUserProfile(Soaring.Player.CustomData, null);
				AnalyticsWrapper.LogPatchyChestPickup(simulation.game, simulated, reward);
			}

			// Token: 0x0600170D RID: 5901 RVA: 0x0009AA40 File Offset: 0x00098C40
			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<TreasureEntity>().ClearingReward.GenerateReward(simulation, false);
			}
		}

		// Token: 0x02000307 RID: 775
		public class BuriedStateFriend : Simulated.StateAction
		{
			// Token: 0x0600170F RID: 5903 RVA: 0x0009AA5C File Offset: 0x00098C5C
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState("inactive");
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
			}

			// Token: 0x06001710 RID: 5904 RVA: 0x0009AA9C File Offset: 0x00098C9C
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.particleSystemManager.RemoveRequestWithDelegate(simulated.starsParticleSystemRequestDelegate);
				simulated.starsParticleSystemRequestDelegate.isAssigned = false;
			}

			// Token: 0x06001711 RID: 5905 RVA: 0x0009AABC File Offset: 0x00098CBC
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (!simulated.starsParticleSystemRequestDelegate.isAssigned)
				{
					simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Sparkles_Rising2", 0, 0, 1f, simulated.starsParticleSystemRequestDelegate);
					simulated.starsParticleSystemRequestDelegate.isAssigned = true;
				}
				return false;
			}
		}
	}

	// Token: 0x02000308 RID: 776
	public class Wanderer
	{
		// Token: 0x06001714 RID: 5908 RVA: 0x0009AB58 File Offset: 0x00098D58
		public static Simulated Load(ResidentEntity residentEntity, ulong? hideExpiresAt, bool? disableFlee, Simulation simulation, ulong utcNow)
		{
			Simulated.StateAction stateAction = EntityManager.WandererActions["hidden"];
			TFUtils.DebugLog(string.Concat(new object[]
			{
				"Loading wanderer(name=",
				(string)residentEntity.Invariable["name"],
				", id=",
				residentEntity.Id,
				", did=",
				residentEntity.DefinitionId,
				", state=",
				stateAction.ToString()
			}));
			Vector2 position = simulation.GetRandomWaypoint().Position;
			Simulated simulated = simulation.CreateSimulated(residentEntity, stateAction, position);
			simulated.Visible = false;
			simulated.IsSwarmManaged = false;
			residentEntity.Wanderer = true;
			residentEntity.HideExpiresAt = hideExpiresAt;
			residentEntity.DisableFlee = disableFlee;
			return simulated;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0009AC18 File Offset: 0x00098E18
		public static void AddWandererToGameState(Dictionary<string, object> gameState, string wandererId, int wandererDid)
		{
			List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["wanderers"];
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did"] = wandererDid;
			dictionary["label"] = wandererId;
			list.Add(dictionary);
		}

		// Token: 0x04000F56 RID: 3926
		public static Simulated.Wanderer.SpawnState Spawn = new Simulated.Wanderer.SpawnState();

		// Token: 0x04000F57 RID: 3927
		public static Simulated.Wanderer.HiddenState Hidden = new Simulated.Wanderer.HiddenState();

		// Token: 0x04000F58 RID: 3928
		public static Simulated.Wanderer.IdleState Idle = new Simulated.Wanderer.IdleState();

		// Token: 0x04000F59 RID: 3929
		public static Simulated.Wanderer.WanderingState Wandering = new Simulated.Wanderer.WanderingState();

		// Token: 0x04000F5A RID: 3930
		public static Simulated.Wanderer.ClickedState Clicked = new Simulated.Wanderer.ClickedState();

		// Token: 0x04000F5B RID: 3931
		public static Simulated.Wanderer.FleeingState Fleeing = new Simulated.Wanderer.FleeingState();

		// Token: 0x04000F5C RID: 3932
		public static Simulated.Wanderer.CheeringState Cheering = new Simulated.Wanderer.CheeringState();

		// Token: 0x02000309 RID: 777
		public class SpawnState : Simulated.StateAction
		{
			// Token: 0x06001717 RID: 5911 RVA: 0x0009AC78 File Offset: 0x00098E78
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.DisableIfWillFlee.Value && !entity.DisableFlee.Value)
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
			}

			// Token: 0x06001718 RID: 5912 RVA: 0x0009ACF0 File Offset: 0x00098EF0
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x06001719 RID: 5913 RVA: 0x0009ACF4 File Offset: 0x00098EF4
			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				Waypoint randomWaypoint = simulation.GetRandomWaypoint();
				if (randomWaypoint == null)
				{
					return false;
				}
				Vector2 position = randomWaypoint.Position;
				simulated.Warp(position, simulation);
				simulated.Visible = true;
				simulation.Router.Send(WanderCommand.Create(simulated.Id, simulated.Id));
				return false;
			}
		}

		// Token: 0x0200030A RID: 778
		public class HiddenState : Simulated.StateAction
		{
			// Token: 0x0600171B RID: 5915 RVA: 0x0009AD4C File Offset: 0x00098F4C
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				simulated.InteractionState.Clear();
				simulated.Visible = false;
				simulated.Variable["pathing"] = null;
				simulated.DisplayThoughtState(null, simulation);
				simulated.ClearPathInfo();
			}

			// Token: 0x0600171C RID: 5916 RVA: 0x0009AD98 File Offset: 0x00098F98
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x0600171D RID: 5917 RVA: 0x0009AD9C File Offset: 0x00098F9C
			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.DisableIfWillFlee.Value && !entity.DisableFlee.Value)
				{
					return false;
				}
				if (entity.HideExpiresAt != null)
				{
					ulong? hideExpiresAt = entity.HideExpiresAt;
					if (hideExpiresAt == null || hideExpiresAt.Value > TFUtils.EpochTime())
					{
						return false;
					}
				}
				simulation.Router.Send(SpawnCommand.Create(simulated.Id, simulated.Id, simulated.GetEntity<ResidentEntity>().BlueprintName));
				return false;
			}
		}

		// Token: 0x0200030B RID: 779
		public class IdleState : Simulated.StateAction
		{
			// Token: 0x0600171F RID: 5919 RVA: 0x0009AE48 File Offset: 0x00099048
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.DisplayState("idle");
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.GetEntity<ResidentEntity>().StartCheckForResume();
			}

			// Token: 0x06001720 RID: 5920 RVA: 0x0009AE94 File Offset: 0x00099094
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().StopCheckForResume();
				simulated.InteractionState.Clear();
			}

			// Token: 0x06001721 RID: 5921 RVA: 0x0009AEAC File Offset: 0x000990AC
			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (simulated.GetEntity<ResidentEntity>().CheckForResume())
				{
					simulation.Router.Send(WanderCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}
		}

		// Token: 0x0200030C RID: 780
		public class WanderingState : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x06001723 RID: 5923 RVA: 0x0009AEF0 File Offset: 0x000990F0
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.SetInteractions(false, false, false, true, null, null);
				simulated.DisplayState("walk");
				simulated.DisplayThoughtState(null, simulation);
				Simulated.FollowingPath.GetWaypointPath(simulation, simulated);
				simulated.GetEntity<ResidentEntity>().StartCheckForIdle();
			}

			// Token: 0x06001724 RID: 5924 RVA: 0x0009AF44 File Offset: 0x00099144
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.Variable["pathing"] = null;
				simulated.ClearPathInfo();
				simulated.Warp(simulated.Position, null);
				simulated.GetEntity<ResidentEntity>().StopCheckForIdle();
			}

			// Token: 0x06001725 RID: 5925 RVA: 0x0009AF80 File Offset: 0x00099180
			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.RandomWanderSimulate(simulation, simulated);
				if (simulated.GetEntity<ResidentEntity>().CheckForIdle())
				{
					simulation.Router.Send(IdlePauseCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}
		}

		// Token: 0x0200030D RID: 781
		public class ClickedState : Simulated.StateAction
		{
			// Token: 0x06001727 RID: 5927 RVA: 0x0009AFCC File Offset: 0x000991CC
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.Clear();
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.DisableFlee != null && entity.DisableFlee.Value)
				{
					simulation.Router.Send(CheerCommand.Create(simulated.Id, simulated.Id));
				}
				else
				{
					simulation.Router.Send(FleeCommand.Create(simulated.Id, simulated.Id));
				}
				simulation.ModifyGameStateSimulated(simulated, new TapWandererAction(simulated.Id, entity.DefinitionId));
			}

			// Token: 0x06001728 RID: 5928 RVA: 0x0009B07C File Offset: 0x0009927C
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x06001729 RID: 5929 RVA: 0x0009B080 File Offset: 0x00099280
			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		// Token: 0x0200030E RID: 782
		public class FleeingState : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x0600172B RID: 5931 RVA: 0x0009B08C File Offset: 0x0009928C
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.Clear();
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, simulation.GetRandomWaypoint().Position);
				simulated.ClearPathInfo();
				simulated.DisplayState("walk");
			}

			// Token: 0x0600172C RID: 5932 RVA: 0x0009B0EC File Offset: 0x000992EC
			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x0600172D RID: 5933 RVA: 0x0009B0F0 File Offset: 0x000992F0
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (base.FollowPathSimulate(simulation, simulated))
				{
					ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
					entity.HideExpiresAt = new ulong?(TFUtils.EpochTime() + (ulong)((long)entity.HideDuration));
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
					simulation.ModifyGameStateSimulated(simulated, new HideWandererAction(simulated.Id, entity.DefinitionId, entity.HideExpiresAt.Value));
				}
				return false;
			}

			// Token: 0x0600172E RID: 5934 RVA: 0x0009B174 File Offset: 0x00099374
			protected override float GetSpeedAddition(Simulated simulated)
			{
				return (float)simulated.Variable["speed"] * 6f;
			}
		}

		// Token: 0x0200030F RID: 783
		public abstract class TransitionallyAnimating : Simulated.StateActionDefault
		{
			// Token: 0x06001730 RID: 5936 RVA: 0x0009B19C File Offset: 0x0009939C
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState(this.DisplayStateName);
				string displayThoughtMaterial = this.GetDisplayThoughtMaterial(simulation, simulated);
				if (displayThoughtMaterial != null)
				{
					simulated.DisplayThoughtState(displayThoughtMaterial, this.DisplayThoughtStateName, simulation);
				}
				else
				{
					simulated.DisplayThoughtState(this.DisplayThoughtStateName, simulation);
				}
				simulated.InteractionState.Clear();
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), (ulong)((long)this.AnimationLength));
			}

			// Token: 0x06001731 RID: 5937 RVA: 0x0009B228 File Offset: 0x00099428
			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				simulated.DisplayThoughtState(null, simulation);
			}

			// Token: 0x17000313 RID: 787
			// (get) Token: 0x06001732 RID: 5938
			protected abstract string DisplayStateName { get; }

			// Token: 0x17000314 RID: 788
			// (get) Token: 0x06001733 RID: 5939
			protected abstract string DisplayThoughtStateName { get; }

			// Token: 0x06001734 RID: 5940
			protected abstract string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated);

			// Token: 0x17000315 RID: 789
			// (get) Token: 0x06001735 RID: 5941
			protected abstract int AnimationLength { get; }
		}

		// Token: 0x02000310 RID: 784
		public class CheeringState : Simulated.Wanderer.TransitionallyAnimating
		{
			// Token: 0x17000316 RID: 790
			// (get) Token: 0x06001737 RID: 5943 RVA: 0x0009B248 File Offset: 0x00099448
			protected override string DisplayStateName
			{
				get
				{
					return "cheer";
				}
			}

			// Token: 0x17000317 RID: 791
			// (get) Token: 0x06001738 RID: 5944 RVA: 0x0009B250 File Offset: 0x00099450
			protected override string DisplayThoughtStateName
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06001739 RID: 5945 RVA: 0x0009B254 File Offset: 0x00099454
			protected override string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated)
			{
				return null;
			}

			// Token: 0x17000318 RID: 792
			// (get) Token: 0x0600173A RID: 5946 RVA: 0x0009B258 File Offset: 0x00099458
			protected override int AnimationLength
			{
				get
				{
					return 2;
				}
			}
		}
	}

	// Token: 0x02000311 RID: 785
	public class Worker
	{
		// Token: 0x04000F5D RID: 3933
		public static Simulated.Worker.IdleState Idle = new Simulated.Worker.IdleState();

		// Token: 0x04000F5E RID: 3934
		public static Simulated.Worker.MovingState Moving = new Simulated.Worker.MovingState();

		// Token: 0x04000F5F RID: 3935
		public static Simulated.Worker.ReturningState Returning = new Simulated.Worker.ReturningState();

		// Token: 0x04000F60 RID: 3936
		public static Simulated.Worker.ErectingState Erecting = new Simulated.Worker.ErectingState();

		// Token: 0x02000312 RID: 786
		public class IdleState : Simulated.StateAction
		{
			// Token: 0x0600173E RID: 5950 RVA: 0x0009B2A4 File Offset: 0x000994A4
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.Warp(simulated.Position, null);
				simulated.DisplayState("idle");
				simulated.DisplayThoughtState(null, simulation);
			}

			// Token: 0x0600173F RID: 5951 RVA: 0x0009B2D8 File Offset: 0x000994D8
			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x06001740 RID: 5952 RVA: 0x0009B2DC File Offset: 0x000994DC
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		// Token: 0x02000313 RID: 787
		public class MovingState : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x06001742 RID: 5954 RVA: 0x0009B2E8 File Offset: 0x000994E8
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, (Vector2)simulated.command["position"]);
				simulated.ClearPathInfo();
				simulated.command = null;
				simulated.DisplayState("walk");
				simulated.DisplayThoughtState(null, simulation);
			}

			// Token: 0x06001743 RID: 5955 RVA: 0x0009B348 File Offset: 0x00099548
			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x06001744 RID: 5956 RVA: 0x0009B34C File Offset: 0x0009954C
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (base.FollowPathSimulate(simulation, simulated))
				{
					simulation.Router.Send(ErectCommand.Create(simulated.Id, simulated.Id, Identity.Null(), 0UL));
				}
				return false;
			}
		}

		// Token: 0x02000314 RID: 788
		public class ReturningState : Simulated.FollowingPath, Simulated.StateAction
		{
			// Token: 0x06001746 RID: 5958 RVA: 0x0009B394 File Offset: 0x00099594
			public void Enter(Simulation simulation, Simulated simulated)
			{
				Simulated closestWorkerSpawner = simulation.GetClosestWorkerSpawner(simulated.Position);
				Vector2 goal = simulated.Position;
				if (closestWorkerSpawner != null)
				{
					goal = closestWorkerSpawner.PointOfInterest;
				}
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, goal);
				simulated.ClearPathInfo();
				simulated.command = null;
				simulated.DisplayState("walk");
				simulated.DisplayThoughtState(null, simulation);
			}

			// Token: 0x06001747 RID: 5959 RVA: 0x0009B400 File Offset: 0x00099600
			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			// Token: 0x06001748 RID: 5960 RVA: 0x0009B404 File Offset: 0x00099604
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return base.FollowPathSimulate(simulation, simulated);
			}
		}

		// Token: 0x02000315 RID: 789
		public class ErectingState : Simulated.StateAction
		{
			// Token: 0x0600174A RID: 5962 RVA: 0x0009B420 File Offset: 0x00099620
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulation.soundEffectManager.PlaySound("Construction");
				simulated.DisplayState("work");
				simulated.DisplayThoughtState(null, simulation);
				TFUtils.DebugLog(string.Concat(new string[]
				{
					"Worker(",
					simulated.Id.Describe(),
					"):Erecting(",
					(simulated.command["building"] as Identity).Describe(),
					")"
				}));
				simulated.command = null;
			}

			// Token: 0x0600174B RID: 5963 RVA: 0x0009B4AC File Offset: 0x000996AC
			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
			}

			// Token: 0x0600174C RID: 5964 RVA: 0x0009B4BC File Offset: 0x000996BC
			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}
	}

	// Token: 0x02000316 RID: 790
	public interface StateAction
	{
		// Token: 0x0600174D RID: 5965
		void Enter(Simulation simulation, Simulated simulated);

		// Token: 0x0600174E RID: 5966
		void Leave(Simulation simulation, Simulated simulated);

		// Token: 0x0600174F RID: 5967
		bool Simulate(Simulation simulation, Simulated simulated, Session session);
	}

	// Token: 0x02000317 RID: 791
	public abstract class StateActionDefault : Simulated.StateAction
	{
		// Token: 0x06001751 RID: 5969
		public abstract void Enter(Simulation simulation, Simulated simulated);

		// Token: 0x06001752 RID: 5970 RVA: 0x0009B4C8 File Offset: 0x000996C8
		public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
		{
			return false;
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x0009B4CC File Offset: 0x000996CC
		public virtual void Leave(Simulation simulation, Simulated simulated)
		{
		}
	}

	// Token: 0x02000318 RID: 792
	public abstract class StateActionBuildingDefault : Simulated.StateActionDefault
	{
		// Token: 0x06001755 RID: 5973 RVA: 0x0009B4D8 File Offset: 0x000996D8
		public virtual void UpdateControls(Simulation simulation, Simulated simulated)
		{
		}
	}

	// Token: 0x02000319 RID: 793
	public abstract class RushingSomething : Simulated.StateActionDefault
	{
		// Token: 0x06001757 RID: 5975 RVA: 0x0009B4E4 File Offset: 0x000996E4
		public virtual void CancelCurrentCommands(Simulation simulation, Simulated simulated)
		{
			int num = simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id, null);
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x0009B50C File Offset: 0x0009970C
		public override void Enter(Simulation simulation, Simulated simulated)
		{
			simulated.command = null;
			simulated.InteractionState.Clear();
			this.CancelCurrentCommands(simulation, simulated);
			simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
			Cost cost = new Cost();
			cost += this.GetRushCost(simulation, simulated);
			cost.Prorate((float)simulated.Variable[Simulated.RUSH_PERCENT]);
			ResourceManager resourceManager = simulation.resourceManager;
			if (resourceManager.CanPay(cost))
			{
				resourceManager.Apply(new Cost(cost), simulation.game);
			}
			else
			{
				TFUtils.Assert(false, "You don't have enough money! Consider showing an insufficient funds dialog before getting here!");
			}
			simulated.rushParameters = null;
		}

		// Token: 0x06001759 RID: 5977
		protected abstract Cost GetRushCost(Simulation simulation, Simulated simulated);
	}

	// Token: 0x0200031A RID: 794
	public class RushParameters
	{
		// Token: 0x0600175A RID: 5978 RVA: 0x0009B5BC File Offset: 0x000997BC
		public RushParameters(Action<Session> execute, Action<Session> cancel, Cost.CostAtTime cost, string subject, int did, Action<Session, Cost, bool> log, Vector2 screenPosition)
		{
			this.execute = execute;
			this.cost = cost;
			this.cancel = cancel;
			this.subject = subject;
			this.log = log;
			this.screenPosition = screenPosition;
			this.did = did;
		}

		// Token: 0x04000F61 RID: 3937
		public Cost.CostAtTime cost;

		// Token: 0x04000F62 RID: 3938
		public string subject;

		// Token: 0x04000F63 RID: 3939
		public int did;

		// Token: 0x04000F64 RID: 3940
		public Action<Session> execute;

		// Token: 0x04000F65 RID: 3941
		public Action<Session> cancel;

		// Token: 0x04000F66 RID: 3942
		public Action<Session, Cost, bool> log;

		// Token: 0x04000F67 RID: 3943
		public Vector2 screenPosition;
	}

	// Token: 0x0200031B RID: 795
	public interface Animated
	{
		// Token: 0x0600175B RID: 5979
		Vector3 Animate(Simulation simulation, Simulated simulated);
	}

	// Token: 0x0200031C RID: 796
	[Flags]
	public enum SimulatedFlags
	{
		// Token: 0x04000F69 RID: 3945
		MOBILE = 1,
		// Token: 0x04000F6A RID: 3946
		BUILDING_ANIM_PATH = 2,
		// Token: 0x04000F6B RID: 3947
		FIRST_ANIMATE = 4,
		// Token: 0x04000F6C RID: 3948
		FORCE_ANIMATE_ACTION = 8,
		// Token: 0x04000F6D RID: 3949
		FORCE_ANIMATE_FOOTPRINT = 16,
		// Token: 0x04000F6E RID: 3950
		FORCE_ANIMATE_BOUNCE = 32,
		// Token: 0x04000F6F RID: 3951
		FORCE_ANIMATE_BOUNCE_START = 64,
		// Token: 0x04000F70 RID: 3952
		FORCE_ANIMATE_BOUNCE_END = 128
	}

	// Token: 0x0200031D RID: 797
	public struct PendingCommand
	{
		// Token: 0x04000F71 RID: 3953
		public Command c;

		// Token: 0x04000F72 RID: 3954
		public float? delay;
	}

	// Token: 0x0200031E RID: 798
	public class ParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		// Token: 0x0600175C RID: 5980 RVA: 0x0009B5FC File Offset: 0x000997FC
		public ParticleSystemRequestDelegate(Simulated simulated)
		{
			this.simulated = simulated;
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x0009B60C File Offset: 0x0009980C
		public virtual Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x0009B610 File Offset: 0x00099810
		public virtual Vector3 Position
		{
			get
			{
				if (this.simulated.particleDisplayOffsetWorld != null)
				{
					return this.simulated.displayController.Position + this.simulated.particleDisplayOffsetWorld.Value;
				}
				return this.simulated.thoughtDisplayController.Position;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x0600175F RID: 5983 RVA: 0x0009B668 File Offset: 0x00099868
		public virtual bool isVisible
		{
			get
			{
				return this.simulated.displayController.isVisible;
			}
		}

		// Token: 0x04000F73 RID: 3955
		protected Simulated simulated;
	}

	// Token: 0x0200031F RID: 799
	public class RewardParticleRequestDelegate : Simulated.ParticleSystemRequestDelegate
	{
		// Token: 0x06001760 RID: 5984 RVA: 0x0009B67C File Offset: 0x0009987C
		public RewardParticleRequestDelegate(Simulated theSimulated) : base(theSimulated)
		{
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x0009B688 File Offset: 0x00099888
		public override Vector3 Position
		{
			get
			{
				return this.simulated.thoughtItemBubbleDisplayController.Position;
			}
		}
	}

	// Token: 0x02000320 RID: 800
	public class ThoughtBubblePopParticleRequestDelegate : Simulated.RewardParticleRequestDelegate
	{
		// Token: 0x06001762 RID: 5986 RVA: 0x0009B69C File Offset: 0x0009989C
		public ThoughtBubblePopParticleRequestDelegate(Simulated theSimulated) : base(theSimulated)
		{
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x0009B6A8 File Offset: 0x000998A8
		public override bool isVisible
		{
			get
			{
				return true;
			}
		}
	}

	// Token: 0x02000321 RID: 801
	public class EatParticleRequestDelegate : Simulated.RewardParticleRequestDelegate
	{
		// Token: 0x06001764 RID: 5988 RVA: 0x0009B6AC File Offset: 0x000998AC
		public EatParticleRequestDelegate(Simulated simulated) : base(simulated)
		{
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06001765 RID: 5989 RVA: 0x0009B6B8 File Offset: 0x000998B8
		public override Vector3 Position
		{
			get
			{
				Paperdoll paperdoll = (Paperdoll)this.simulated.displayController;
				Transform transform = paperdoll.GetBone("BN_MOUTH_OPENMOUTH");
				if (transform == null)
				{
					transform = paperdoll.GetBone("BN_HEAD");
				}
				if (transform == null)
				{
					transform = paperdoll.GetBone("BN_HIP");
				}
				if (transform == null)
				{
					transform = paperdoll.GetBone("BN_ROOT");
				}
				if (transform == null)
				{
					transform = paperdoll.Transform;
				}
				return transform.position;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x0009B744 File Offset: 0x00099944
		public override bool isVisible
		{
			get
			{
				return true;
			}
		}
	}

	// Token: 0x02000322 RID: 802
	public class ActivateParticleRequestDelegate : Simulated.ParticleSystemRequestDelegate
	{
		// Token: 0x06001767 RID: 5991 RVA: 0x0009B748 File Offset: 0x00099948
		public ActivateParticleRequestDelegate(Simulated theSimulated) : base(theSimulated)
		{
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x0009B754 File Offset: 0x00099954
		public override Vector3 Position
		{
			get
			{
				return this.simulated.displayController.Position;
			}
		}
	}

	// Token: 0x02000323 RID: 803
	public class SimulatedParticleRequestDelegate : Simulated.ParticleSystemRequestDelegate
	{
		// Token: 0x06001769 RID: 5993 RVA: 0x0009B768 File Offset: 0x00099968
		public SimulatedParticleRequestDelegate(Simulated theSimulated) : base(theSimulated)
		{
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x0600176A RID: 5994 RVA: 0x0009B78C File Offset: 0x0009998C
		public override Vector3 Position
		{
			get
			{
				Vector3 vector = new Vector3(this.simulated.Position.x, this.simulated.Position.y);
				Vector3 forward = this.simulated.displayController.Forward;
				vector += forward;
				vector += this.offset;
				return vector;
			}
		}

		// Token: 0x04000F74 RID: 3956
		public bool isAssigned;

		// Token: 0x04000F75 RID: 3957
		public Vector3 offset = new Vector3(0f, 0f, 20f);
	}

	// Token: 0x02000324 RID: 804
	public struct TimebarMixinArgs
	{
		// Token: 0x04000F76 RID: 3958
		public bool hasTimebar;

		// Token: 0x04000F77 RID: 3959
		public string description;

		// Token: 0x04000F78 RID: 3960
		public ulong completeTime;

		// Token: 0x04000F79 RID: 3961
		public ulong totalTime;

		// Token: 0x04000F7A RID: 3962
		public float duration;

		// Token: 0x04000F7B RID: 3963
		public Cost rushCost;

		// Token: 0x04000F7C RID: 3964
		public bool m_bCheckForTaskCharacters;
	}

	// Token: 0x02000325 RID: 805
	public struct NamebarMixinArgs
	{
		// Token: 0x04000F7D RID: 3965
		public bool m_bHasNamebar;

		// Token: 0x04000F7E RID: 3966
		public string m_sName;

		// Token: 0x04000F7F RID: 3967
		public bool m_bCheckForTaskCharacters;
	}
}
