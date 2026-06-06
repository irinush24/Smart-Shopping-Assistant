export interface CartItem {
  id: number
  productId: number
  productName: string
  unitPrice: number
  quantity: number
  subtotal: number
}

export interface AppliedPromotion {
  promotionName: string
  discountAmount: number
  discountLabel: string
}

export interface CartModel {
  items: CartItem[]
  subtotal: number
  appliedPromotions: AppliedPromotion[]
  totalDiscount: number
  total: number
}

export interface AddCartItemInput {
  productId: number
  quantity: number
}

export interface UpdateCartItemInput {
  quantity: number
}