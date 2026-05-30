import type { PromotionModel, PromotionType, PromotionReward } from "../../../api/models/PromotionModel"

export interface Promotion{
    id: number,
    name: string,
    promotionType: PromotionType,
    promotionReward: PromotionReward,
    threshold: number,
    rewardValue: number,
    isActive: boolean
}

export function toPromotion(dto: PromotionModel) : Promotion
{
    return{
        id: dto.id,
        name: dto.name,
        promotionType: dto.promotionType,
        promotionReward: dto.promotionReward,
        threshold: dto.threshold,
        rewardValue: dto.rewardValue,
        isActive: dto.isActive
    }
}