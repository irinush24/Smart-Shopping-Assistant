import { toProduct, type Product } from "../../components/shared/types/Product"
import type { ProductModel, ProductInput } from "../models/ProductModel"
import {http} from "../base/http"

export const productsApi = {
    getAll: async(): Promise<Product[]> => {
        const data = await http.get<ProductModel[]>('/Products')
        return data.map(toProduct)
    },

    getById: async (id: number): Promise<Product> =>{
        const data = await http.get<ProductModel>(`/Products/${id}`)
        return toProduct(data)
    },

    create: async (data: ProductInput): Promise<Product> => {
        return toProduct(await http.post<ProductModel>('/Products', data))
    },

    update: async (id: number, data: ProductInput): Promise<Product> => {
        return toProduct(await http.put<ProductModel>(`/Products/${id}`, data))
    },

    remove: (id: number) => http.remove<void>(`/Products/${id}`),
}