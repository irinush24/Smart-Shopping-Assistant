import { toPromotion, type Promotion} from "../../components/shared/types/Promotion"
import type { PromotionModel, PromotionInput } from "../models/PromotionModel"
import {http} from "../base/http"

export const promotionsApi = {
    getAll: async(): Promise<Promotion[]> => {
        const data = await http.get<PromotionModel[]>('/Promotion')
        return data.map(toPromotion)
    },

    getById: async (id: number): Promise<Promotion> =>{
        const data = await http.get<PromotionModel>(`/Promotion/${id}`)
        return toPromotion(data)
    },

    create: async (data: PromotionInput): Promise <Promotion> => {
        return toPromotion(await http.post<PromotionModel>('/Promotion', data))
    },

    update: async (id: number, data: PromotionInput): Promise<Promotion> => {
        return toPromotion(await http.put<PromotionModel>(`/Promotion/${id}`, data))
    },

    remove: (id: number) => http.remove<void>(`/Promotion/${id}`),
}