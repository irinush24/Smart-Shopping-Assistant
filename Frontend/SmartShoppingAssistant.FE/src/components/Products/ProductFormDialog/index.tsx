import {Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, Stack, TextField} from "@mui/material"
import type { Product } from "../../shared/types/Product"
import { useState } from "react"
import { productsApi } from "../../../api/clients/ProductApiClient"
import type { ProductInput } from "../../../api/models/ProductModel"

interface ProductFormDialogProps {
    product: Product | null
    onClose: () => void
    onSaved: () => void
}

function ProductFormDialog ({
    product,
    onClose,
    onSaved
} : ProductFormDialogProps) {
    const isEditing = product !== null

    const [name, setName] = useState(product?.name ?? '')
    const [description, setDescription] = useState(product?.description ?? '')
    const [price, setPrice] = useState<string | number>(product?.price ?? '')

    const [error, setError] = useState('')
    const [saving, setSaving] = useState(false)

    async function handleSave() {
        if(name.trim() === ''){
            setError('Name is required.')
            return
        }
        if(price === '' || isNaN(Number(price)) || Number(price) < 0){
            setError('Invalid price format. Non-negative number expected.')
            return
        }
        setSaving(true)
        setError('')
        try {
            const data : ProductInput = { name, description, price : Number(price) }
            if(isEditing) {
                await productsApi.update(product.id, data)
            } else {
                await productsApi.create(data)
            }
            onSaved()
        } catch(err) {
            setError((err as Error).message)
            setSaving(false)
        }
    }

    return (
        <Dialog open onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle>{isEditing ? "Edit Product" : "Add Product"}</DialogTitle>
            <DialogContent>
                <Stack spacing = {2} sx = {{mt: 1}}>
                    {error !== "" && <Alert severity="error">{error}</Alert>}
                    <TextField
                        label = "Name"
                        value = {name}
                        onChange={(e) => setName(e.target.value)}
                        fullWidth
                    />
                    <TextField 
                        label = "Description"
                        value = {description}
                        onChange={(e) => setDescription(e.target.value)}
                        fullWidth
                    />
                    <TextField
                        label = "Price"
                        value = {price}
                        onChange={(e) => setPrice(e.target.value)}
                        fullWidth
                    />
                </Stack>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} disabled={saving}>Cancel</Button> 
                <Button onClick={handleSave} disabled={saving} variant="contained">Save</Button>
            </DialogActions>
        </Dialog>
    )
}

export default ProductFormDialog;