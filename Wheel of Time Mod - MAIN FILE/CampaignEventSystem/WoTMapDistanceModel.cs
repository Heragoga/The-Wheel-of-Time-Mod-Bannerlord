using System;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using WoT_Main.Support;

namespace WoT_Main
{
	// Token: 0x02000321 RID: 801
	public class WoTMapDistanceModel : MapDistanceModel
	{
        public WoTMapDistanceModel()
        {
			campaignSupport.displayMessageInChat("MapDistanceModel loaded", Colors.Green);
        }
		// Token: 0x06002EEF RID: 12015 RVA: 0x000BF134 File Offset: 0x000BD334
		public void LoadCacheFromFile(System.IO.BinaryReader reader)
		{
			this._settlementDistanceCache.Clear();
			if (reader == null)
			{
				for (int i = 0; i < Settlement.All.Count; i++)
				{
					Settlement settlement = Settlement.All[i];
					this._settlementsToConsider.Add(settlement);
					for (int j = i + 1; j < Settlement.All.Count; j++)
					{
						Settlement settlement2 = Settlement.All[j];
						float distance = this.GetDistance(settlement.GatePosition, settlement2.GatePosition, settlement.CurrentNavigationFace, settlement2.CurrentNavigationFace);
						if (settlement.Id.InternalValue <= settlement2.Id.InternalValue)
						{
							this._settlementDistanceCache.Add(new ValueTuple<Settlement, Settlement>(settlement, settlement2), distance);
						}
						else
						{
							this._settlementDistanceCache.Add(new ValueTuple<Settlement, Settlement>(settlement2, settlement), distance);
						}
					}
				}
				return;
			}
			int num = reader.ReadInt32();
			for (int k = 0; k < num; k++)
			{
				Settlement settlement3 = Settlement.Find(reader.ReadString());
				this._settlementsToConsider.Add(settlement3);
				for (int l = k + 1; l < num; l++)
				{
					Settlement settlement4 = Settlement.Find(reader.ReadString());
					float value = reader.ReadSingle();
					if (settlement3.Id.InternalValue <= settlement4.Id.InternalValue)
					{
						this._settlementDistanceCache.Add(new ValueTuple<Settlement, Settlement>(settlement3, settlement4), value);
					}
					else
					{
						this._settlementDistanceCache.Add(new ValueTuple<Settlement, Settlement>(settlement4, settlement3), value);
					}
				}
			}
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000BF2C8 File Offset: 0x000BD4C8
		public override float GetDistance(Settlement fromSettlement, Settlement toSettlement)
		{
			float num;
			if (fromSettlement == toSettlement)
			{
				num = 0f;
			}
			else if (fromSettlement.Id.InternalValue <= toSettlement.Id.InternalValue)
			{
				ValueTuple<Settlement, Settlement> key = new ValueTuple<Settlement, Settlement>(fromSettlement, toSettlement);
				if (!this._settlementDistanceCache.TryGetValue(key, out num))
				{
					num = this.GetDistance(fromSettlement.GatePosition, toSettlement.GatePosition, fromSettlement.CurrentNavigationFace, toSettlement.CurrentNavigationFace);
					this._settlementDistanceCache.Add(key, num);
				}
			}
			else
			{
				ValueTuple<Settlement, Settlement> key2 = new ValueTuple<Settlement, Settlement>(toSettlement, fromSettlement);
				if (!this._settlementDistanceCache.TryGetValue(key2, out num))
				{
					num = this.GetDistance(toSettlement.GatePosition, fromSettlement.GatePosition, toSettlement.CurrentNavigationFace, fromSettlement.CurrentNavigationFace);
					this._settlementDistanceCache.Add(key2, num);
				}
			}
			return num;
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x000BF390 File Offset: 0x000BD590
		public override float GetDistance(MobileParty fromParty, Settlement toSettlement)
		{
			float result;
			if (fromParty.CurrentSettlement != null)
			{
				result = this.GetDistance(fromParty.CurrentSettlement, toSettlement);
			}
			else
			{
				Settlement closestSettlementForNavigationMesh = this.GetClosestSettlementForNavigationMesh(fromParty.CurrentNavigationFace);
				result = fromParty.Position2D.Distance(toSettlement.GatePosition) - closestSettlementForNavigationMesh.GatePosition.Distance(toSettlement.GatePosition) + this.GetDistance(closestSettlementForNavigationMesh, toSettlement);
			}
			return result;
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x000BF3F8 File Offset: 0x000BD5F8
		public override float GetDistance(MobileParty fromParty, MobileParty toParty)
		{
			Settlement settlement = fromParty.CurrentSettlement ?? this.GetClosestSettlementForNavigationMesh(fromParty.CurrentNavigationFace);
			Settlement settlement2 = toParty.CurrentSettlement ?? this.GetClosestSettlementForNavigationMesh(toParty.CurrentNavigationFace);
			return fromParty.Position2D.Distance(toParty.Position2D) - settlement.GatePosition.Distance(settlement2.GatePosition) + this.GetDistance(settlement, settlement2);
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x000BF468 File Offset: 0x000BD668
		public override bool GetDistance(Settlement fromSettlement, Settlement toSettlement, float maximumDistance, out float distance)
		{
			bool flag;
			if (fromSettlement == toSettlement)
			{
				distance = 0f;
				flag = true;
			}
			else if (fromSettlement.Id.InternalValue <= toSettlement.Id.InternalValue)
			{
				ValueTuple<Settlement, Settlement> key = new ValueTuple<Settlement, Settlement>(fromSettlement, toSettlement);
				if (this._settlementDistanceCache.TryGetValue(key, out distance))
				{
					flag = (distance <= maximumDistance);
				}
				else
				{
					flag = this.GetDistanceWithDistanceLimit(fromSettlement.GatePosition, toSettlement.GatePosition, Campaign.Current.MapSceneWrapper.GetFaceIndex(fromSettlement.GatePosition), Campaign.Current.MapSceneWrapper.GetFaceIndex(toSettlement.GatePosition), maximumDistance, out distance);
					if (flag)
					{
						this._settlementDistanceCache.Add(key, distance);
					}
				}
			}
			else
			{
				ValueTuple<Settlement, Settlement> key2 = new ValueTuple<Settlement, Settlement>(toSettlement, fromSettlement);
				if (this._settlementDistanceCache.TryGetValue(key2, out distance))
				{
					flag = (distance <= maximumDistance);
				}
				else
				{
					flag = this.GetDistanceWithDistanceLimit(toSettlement.GatePosition, fromSettlement.GatePosition, Campaign.Current.MapSceneWrapper.GetFaceIndex(toSettlement.GatePosition), Campaign.Current.MapSceneWrapper.GetFaceIndex(fromSettlement.GatePosition), maximumDistance, out distance);
					if (flag)
					{
						this._settlementDistanceCache.Add(key2, distance);
					}
				}
			}
			return flag;
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000BF5A0 File Offset: 0x000BD7A0
		public override bool GetDistance(MobileParty fromParty, Settlement toSettlement, float maximumDistance, out float distance)
		{
			bool result = false;
			if (fromParty.CurrentSettlement != null)
			{
				result = this.GetDistance(fromParty.CurrentSettlement, toSettlement, maximumDistance, out distance);
			}
			else
			{
				Settlement closestSettlementForNavigationMesh = this.GetClosestSettlementForNavigationMesh(fromParty.CurrentNavigationFace);
				if (this.GetDistance(closestSettlementForNavigationMesh, toSettlement, maximumDistance, out distance))
				{
					distance += fromParty.Position2D.Distance(toSettlement.GatePosition) - closestSettlementForNavigationMesh.GatePosition.Distance(toSettlement.GatePosition);
					result = (distance <= maximumDistance);
				}
			}
			return result;
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000BF620 File Offset: 0x000BD820
		public override bool GetDistance(IMapPoint fromMapPoint, MobileParty toParty, float maximumDistance, out float distance)
		{
			Settlement closestSettlementForNavigationMesh = this.GetClosestSettlementForNavigationMesh(fromMapPoint.CurrentNavigationFace);
			Settlement settlement = toParty.CurrentSettlement ?? this.GetClosestSettlementForNavigationMesh(toParty.CurrentNavigationFace);
			bool result = false;
			if (this.GetDistance(closestSettlementForNavigationMesh, settlement, maximumDistance, out distance))
			{
				distance += fromMapPoint.Position2D.Distance(toParty.Position2D) - closestSettlementForNavigationMesh.GatePosition.Distance(settlement.GatePosition);
				result = (distance <= maximumDistance);
			}
			return result;
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000BF69C File Offset: 0x000BD89C
		public override bool GetDistance(IMapPoint fromMapPoint, Settlement toSettlement, float maximumDistance, out float distance)
		{
			bool result = false;
			distance = 100f;
			if (fromMapPoint != null)
			{
				Settlement closestSettlementForNavigationMesh = this.GetClosestSettlementForNavigationMesh(fromMapPoint.CurrentNavigationFace);
				if (this.GetDistance(closestSettlementForNavigationMesh, toSettlement, maximumDistance, out distance))
				{
					distance += fromMapPoint.Position2D.Distance(toSettlement.GatePosition) - closestSettlementForNavigationMesh.GatePosition.Distance(toSettlement.GatePosition);
					result = (distance <= maximumDistance);
				}
			}
			return result;
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000BF70C File Offset: 0x000BD90C
		public override bool GetDistance(IMapPoint fromMapPoint, in Vec2 toPoint, float maximumDistance, out float distance)
		{
			Settlement closestSettlementForNavigationMesh = this.GetClosestSettlementForNavigationMesh(fromMapPoint.CurrentNavigationFace);
			Settlement closestSettlementForPosition = this.GetClosestSettlementForPosition(toPoint);
			bool result = false;
			if (this.GetDistance(closestSettlementForNavigationMesh, closestSettlementForPosition, maximumDistance, out distance))
			{
				distance += fromMapPoint.Position2D.Distance(toPoint) - closestSettlementForNavigationMesh.GatePosition.Distance(closestSettlementForPosition.GatePosition);
				result = (distance <= maximumDistance);
			}
			return result;
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x000BF778 File Offset: 0x000BD978
		private float GetDistance(Vec2 pos1, Vec2 pos2, PathFaceRecord faceIndex1, PathFaceRecord faceIndex2)
		{
			float result;
			Campaign.Current.MapSceneWrapper.GetPathDistanceBetweenAIFaces(faceIndex1, faceIndex2, pos1, pos2, 0.1f, float.MaxValue, out result);
			return result;
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x000BF7A7 File Offset: 0x000BD9A7
		private bool GetDistanceWithDistanceLimit(Vec2 pos1, Vec2 pos2, PathFaceRecord faceIndex1, PathFaceRecord faceIndex2, float distanceLimit, out float distance)
		{
			if (pos1.DistanceSquared(pos2) > distanceLimit * distanceLimit)
			{
				distance = float.MaxValue;
				return false;
			}
			return Campaign.Current.MapSceneWrapper.GetPathDistanceBetweenAIFaces(faceIndex1, faceIndex2, pos1, pos2, 0.1f, distanceLimit, out distance);
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x000BF7E0 File Offset: 0x000BD9E0
		private Settlement GetClosestSettlementForNavigationMesh(PathFaceRecord face)
		{
			Settlement settlement;
			if (!this._navigationMeshClosestSettlementCache.TryGetValue(face.FaceIndex, out settlement))
			{
				Vec2 navigationMeshCenterPosition = Campaign.Current.MapSceneWrapper.GetNavigationMeshCenterPosition(face);
				float num = float.MaxValue;
				foreach (Settlement settlement2 in this._settlementsToConsider)
				{
					float num2 = settlement2.GatePosition.DistanceSquared(navigationMeshCenterPosition);
					if (num > num2)
					{
						num = num2;
						settlement = settlement2;
					}
				}
				this._navigationMeshClosestSettlementCache[face.FaceIndex] = settlement;
			}
			return settlement;
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x000BF88C File Offset: 0x000BDA8C
		private Settlement GetClosestSettlementForPosition(in Vec2 position)
		{
			PathFaceRecord faceIndex = Campaign.Current.MapSceneWrapper.GetFaceIndex(position);
			return this.GetClosestSettlementForNavigationMesh(faceIndex);
		}

		// Token: 0x04000F6C RID: 3948
		private readonly Dictionary<ValueTuple<Settlement, Settlement>, float> _settlementDistanceCache = new Dictionary<ValueTuple<Settlement, Settlement>, float>();

		// Token: 0x04000F6D RID: 3949
		private readonly Dictionary<int, Settlement> _navigationMeshClosestSettlementCache = new Dictionary<int, Settlement>();

		// Token: 0x04000F6E RID: 3950
		private List<Settlement> _settlementsToConsider = new List<Settlement>();
	}
}
