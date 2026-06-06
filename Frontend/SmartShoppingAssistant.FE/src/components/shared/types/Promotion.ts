import type { PromotionModel } from "../../../api/models/PromotionModel"

export interface Promotion {
    id: number;
    name: string;
    type: string;
    typeLabel: string;        // Added back for the UI Table!
    reward: string;
    rewardLabel: string;      // Added back for the UI Table!
    threshold: number;
    rewardValue: number;
    productId: number | null;
    categoryId: number | null;
    isActive: boolean;
}

const typeLabels: Record<string, string> = {
    "Quantity": "Based on the quantity bought of a product",
    "CartTotal": "Based on the cart total"
};

const rewardLabels: Record<string, string> = {
    "FreeItems": "Buy X get Y free",
    "PercentDiscount": "Percentage discount"
};

export function toPromotion(dto: PromotionModel): Promotion {
    return {
        id: dto.id,
        name: dto.name,
        
        type: dto.type, 
        typeLabel: typeLabels[dto.type] ?? "Unknown",
        
        reward: dto.reward, 
        rewardLabel: `${dto.rewardValue} ${rewardLabels[dto.reward] ?? "Unknown"}`,
        
        threshold: dto.threshold,
        rewardValue: dto.rewardValue,
        productId: dto.productId,
        categoryId: dto.categoryId,
        isActive: dto.isActive
    };
}