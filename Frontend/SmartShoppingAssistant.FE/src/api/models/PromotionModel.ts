export type PromotionType = "Quantity" | "CartTotal";
export type PromotionReward = "FreeItems" | "PercentDiscount";

export interface PromotionModel
{
    id: number,
    name: string,
    type: PromotionType | string,
    reward: PromotionReward | string,
    threshold: number,
    rewardValue: number,
    productId: number | null,
    categoryId: number | null,
    isActive: boolean
}

export interface PromotionInput
{
    name: string,
    type: PromotionType | string,
    reward: PromotionReward | string,
    threshold: number,
    rewardValue: number,
    productId: number | null,
    categoryId: number | null,
    isActive: boolean
}