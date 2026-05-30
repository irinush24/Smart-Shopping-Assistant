import { toCategory, type Category } from "../../components/shared/types/Category"
import type { CategoryInput, CategoryModel } from "../models/CategoryModel"
import {http} from "../base/http"


// Component -> categoriesApi -> http -> server

export const categoriesApi = {
    getAll: async(): Promise <Category[]> => {        
        const data = await http.get<CategoryModel[]>('/Category')
        return data.map(toCategory)
    },
    
    // to do get by id
    getById: async (id: number): Promise<Category> => {
        const data = await http.get<CategoryModel>(`/Category/${id}`)
        return toCategory(data)
    },

    create: async (data: CategoryInput): Promise<Category> => {
        return toCategory(await http.post<CategoryModel>('/Category', data))
    },

    update: async (id: number, data: CategoryInput): Promise<Category> => {
        return toCategory(await http.put<CategoryModel>(`/Category/${id}`, data))
    },

    remove: (id: number) => http.remove<void>(`/Category/${id}`),
}