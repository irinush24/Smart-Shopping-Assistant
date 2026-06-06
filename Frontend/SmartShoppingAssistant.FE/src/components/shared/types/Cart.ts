import type { CartModel, AppliedPromotion } from "../../../api/models/CartModel";

function money(value: number | null | undefined): string {
  const safeValue = Number(value) || 0;
  return `${safeValue.toFixed(2)} RON`
}

export interface CartItem {
  id: number
  productName: string
  unitPrice: number
  unitPriceLabel: string
  quantity: number
  subtotal: number
  subtotalLabel: string
}

export interface Cart {
  items: CartItem[]
  subtotal: number
  subtotalLabel: string
  appliedPromotions: AppliedPromotion[]
  totalDiscount: number
  totalDiscountLabel: string
  total: number
  totalLabel: string
  itemCount: number
}

export function toCartModel(dto: CartModel): Cart {
  return {
    items: dto.items.map((item) => ({
      id: item.id,
      productId: item.productId,
      productName: item.productName,
      unitPrice: item.unitPrice,
      unitPriceLabel: money(item.unitPrice),
      quantity: item.quantity,
      subtotal: item.subtotal,
      subtotalLabel: money(item.subtotal),
    })),
    subtotal: dto.subtotal,
    subtotalLabel: money(dto.subtotal),
    appliedPromotions: dto.appliedPromotions.map((promotion) => ({
      promotionName: promotion.promotionName,
      discountAmount: promotion.discountAmount,
      discountLabel: money(promotion.discountAmount),
    })),
    totalDiscount: dto.totalDiscount,
    totalDiscountLabel: money(dto.totalDiscount),
    total: dto.total,
    totalLabel: money(dto.total),
    itemCount: dto.items.reduce((sum, item) => sum + item.quantity, 0),
  }
}