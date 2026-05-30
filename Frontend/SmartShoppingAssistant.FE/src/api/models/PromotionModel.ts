export interface PromotionModel
{
    id: number,
    name: string,
    promotionType: PromotionType,
    promotionReward: PromotionReward,
    threshold: number,
    rewardValue: number,
    isActive: boolean
}

export interface PromotionInput
{
    name: string,
    promotionType: PromotionType,
    promotionReward: PromotionReward,
    threshold: number,
    rewardValue: number,
    isActive: boolean
}

export type PromotionType = "Quantity" | "CartTotal"

export type PromotionReward = "FreeItems" | "PercentDiscount"