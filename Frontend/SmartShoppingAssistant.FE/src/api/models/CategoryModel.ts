// Data received from the server
export interface CategoryModel
{
    id : number
    name: string
    description?: string
}

// Data sent to the server
export interface CategoryInput
{
    name: string,
    description?: string
}