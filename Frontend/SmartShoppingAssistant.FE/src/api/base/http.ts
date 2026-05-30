import axios from "axios";


// Global instance for the whole program
const api = axios.create(
    {
        baseURL: import.meta.env.VITE_API_URL,
        headers: {'Content-Type' : 'application/json'},
    }
)

api.interceptors.response.use(
    // Success case
    (response) => (response),
    // Error case
    (error) => {
        const data = error.response?.data

        // === data type and semantic  == just semantics, type is ignored
        const message = typeof data === 'string' && data != ''? data : error.message || 'Request failed'
        // Promise and Task are basically the same async thing just for different application ends
        return Promise.reject(new Error(message))
    },
)

export const http = {
    get: async <T> (path: string): Promise<T> => {
        const response = await api.get<T>(path)
        return response.data
    },
    post: async <T> (path: string, body: unknown): Promise <T> => {
        const response = await api.post<T>(path, body)
        return response.data
    },
    put:async <T> (path: string, body: unknown): Promise <T> => {
        const response = await api.put<T>(path, body)
        return response.data
    },
    remove: async <T> (path: string): Promise<T> => {
        const response = await api.delete<T>(path)
        return response.data
    },
}