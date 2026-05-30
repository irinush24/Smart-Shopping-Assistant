export interface ProductModel
{
    id: number,
    name: string,
    description?: string,
    imageUrl?: string,
    price: number
}

export interface ProductInput
{
    name: string,
    description?: string,
    imageUrl?: string,
    price: number
}