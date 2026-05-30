import {Alert, Dialog, DialogContent, DialogTitle, TextField, Select, MenuItem, Stack, DialogActions, Button} from "@mui/material"
import type { Promotion } from "../../shared/types/Promotion"
import { useState } from "react"
import { promotionsApi } from "../../../api/clients/PromotionApiClient"
import type { PromotionReward, PromotionType, PromotionInput } from "../../../api/models/PromotionModel"

interface PromotionFormDialogProps {
    promotion: Promotion | null
    onClose: () => void
    onSaved: () => void
}

function PromotionFormDialog ({
    promotion,
    onClose,
    onSaved
} : PromotionFormDialogProps) {
    const isEditing = promotion !== null

    const [name, setName] = useState(promotion?.name ?? '')
    const [threshold, setThreshold] = useState<string | number>(promotion?.threshold ?? '')
    const [rewardValue, setRewardValue] = useState<string | number>(promotion?.rewardValue ?? '')

    // Enums typing and setting default values
    const [promotionType, setPromotionType] = useState<PromotionType>(promotion?.promotionType ?? 'Quantity')
    const [promotionReward, setPromotionReward] = useState<PromotionReward>(promotion?.promotionReward ?? 'PercentDiscount')

    // Keeping isActive as a true boolean
    const [isActive, setIsActive] = useState<boolean>(promotion?.isActive ?? true)
    
    const [error, setError] = useState('')
    const [saving, setSaving] = useState(false)

    async function handleSave() {
        if(name.trim() === ''){
            setError('Name is required.')
            return
        }
        setSaving(true)
        setError('')
        try {
            const data : PromotionInput = { name, promotionType, promotionReward, threshold : Number(threshold), rewardValue : Number(rewardValue), isActive }
            if(isEditing) {
                await promotionsApi.update(promotion.id, data)
            } else {
                await promotionsApi.create(data)
            }
            onSaved()
        } catch(err) {
            setError((err as Error).message)
            setSaving(false)
        }
    }

    return (
        <Dialog open onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle>{isEditing ? "Edit Promotion" : "Add Promotion"}</DialogTitle>
            <DialogContent>
                <Stack spacing = {2} sx = {{mt: 1}}>
                    {error !== "" && <Alert severity="error">{error}</Alert>}
                    <TextField 
                        label = "Name"
                        value = {name}
                        onChange={(e) => setName(e.target.value)}
                        fullWidth
                    />
                    <Select 
                        label = "Type"
                        value = {promotionType}
                        onChange={(e) => setPromotionType(e.target.value as PromotionType)}
                        fullWidth>
                        <MenuItem value = {"Quantity"}>Quantity</MenuItem>
                        <MenuItem value = {"CartTotal"}>Cart Total</MenuItem>
                    </Select>
                    <Select
                        label = "Reward Type"
                        value = {promotionReward}
                        onChange={(e) => setPromotionReward(e.target.value as PromotionReward)}
                        fullWidth>
                        <MenuItem value = {"FreeItems"}>Free Items</MenuItem>
                        <MenuItem value = {"PercentDiscount"}>Percent Discount</MenuItem>
                    </Select>
                    <TextField 
                        label = "Threshold"
                        type = "number"        // Pulls up the number keyboard on mobile
                        value = {threshold}
                        onChange={(e) => setThreshold(e.target.value)}
                        fullWidth
                    />
                    <TextField 
                        label = "Reward Value"
                        type = "number"
                        value = {rewardValue}
                        onChange={(e) => setRewardValue(e.target.value)}
                        fullWidth
                    />
                    <Select
                        label = "Status"
                        value = {isActive? "true" : "false"}            // Map boolean to string for UI
                        onChange={(e) => setIsActive(e.target.value === "true")}        // Map it back to boolean for the state change
                        fullWidth>
                        <MenuItem value = {"true"}>Active</MenuItem>
                        <MenuItem value = {"false"}>Inactive</MenuItem>
                    </Select>
                </Stack>
            </DialogContent>
            <DialogActions>
                <Button onClick = {onClose}>Cancel</Button>
                <Button variant = "contained" onClick = {handleSave} disabled = {saving}>Save</Button>
            </DialogActions> 
        </Dialog>
    );
}

export default PromotionFormDialog;