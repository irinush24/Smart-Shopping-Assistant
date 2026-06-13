import { Alert, Box, Button, Dialog, DialogContent, DialogTitle, Divider, IconButton, LinearProgress, Stack, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import type { Analysis, Suggestion } from "../../shared/types/Analysis";
import { useCart } from "../../../context/CartContext/cart-context"
import { cartApi } from "../../../api/clients/CartApiClient";
import AutoAwesomeIcon from "@mui/icons-material/AutoAwesome";
import CheckIcon from "@mui/icons-material/Check"
import CloseIcon from "@mui/icons-material/Close"
import { CloseOutlined } from "@mui/icons-material";

interface AnalyzeDialogProps{
    onClose: () => void
}

const loadingMessages = [
  '🤖 Reading your cart...',
  '🔍 Checking promotions...',
  '✨ Finding the best deals...',
  '🛒 Composing suggestions...',
]

type Decision = 'approved' | 'declined'

function AnalyzeDialog({onClose}: AnalyzeDialogProps)
{
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState('')
    const [analyze, setAnalyze] = useState<Analysis | null>(null)
    const [decisions, setDecisions] = useState<Record <number, Decision>>({})
    const [messageIndex, setMessageIndex] = useState(0)
    const [progress, setProgress] = useState(0)
        
    const {addItem} = useCart();

    useEffect(() => {
        cartApi.analyze()
        .then((data) =>
        {
            setAnalyze(data)
            setError('')
        })
        .catch((err) => {
            setError((err as Error).message)
        })
        .finally(() => {
            setLoading(false)
        })
    }, [])

    useEffect(() => {
        if(!loading) return
        const timer = setInterval(() =>{
            setProgress((current) => (current >= 90 ? 90 : current + 5))
            setMessageIndex((current) => (current + 1)% loadingMessages.length)
        }, 1000)
        return () => clearInterval(timer)
    }, [loading])

    //... spread operator = de destructurare pe arrays sau pe dictionare, obiecte in sine
    // pe arrays : luam fiecare element de sine statator
    // pe dictionar/obiect la modul general ne ia proprietate cu valoare
    // face un shallow copy la current si ii mai adauga ceva
    // daca exista cheia ii da update
    // daca nu, o adauga

    async function handleApprove(suggestion:Suggestion) {
        await addItem(suggestion.productId, suggestion.quantity)
        setDecisions((current) => ({...current, [suggestion.productId]: "approved"}))
    }

    async function handleDecline(suggestion:Suggestion) {
        await addItem(suggestion.productId, suggestion.quantity)
        setDecisions((current) => ({...current, [suggestion.productId]: "declined"}))
    }

    // Automatic closing of the dialog once the user has accepted or declined all of the promotions
    useEffect(() => {
        if(!analyze || loading) return;

        const actionableSuggestions = analyze.suggestions.filter(s => s.quantity > 0);

        if (actionableSuggestions.length === 0) return;

        const allDecided = actionableSuggestions.every((suggestion) => decisions[suggestion.productId] !== undefined);

        if(allDecided)
        {
            const timer = setTimeout(() => {
                onClose();
            }, 800);        // ms delay for the user to see the final UI update

            return () => clearTimeout(timer);   // cleanup
        }
    }, [decisions, analyze, loading, onClose]);

    return (
        <Dialog open onClose = {onClose}>
            <Box sx ={{display: 'flex', justifyContent: 'space-between', flexDirection: 'row', pt: 1}}>
                <DialogTitle sx = {{display: "flex", alignItems: "center", gap: 1}}>
                <AutoAwesomeIcon color = 'primary'/>
                AI Cart Analysis</DialogTitle>
                <IconButton onClick={onClose} sx={{mr: 2}}>
                    <CloseOutlined fontSize = "medium"/>
                </IconButton>
            </Box>
                <DialogContent>
                    {loading && (<Box sx = {{py: 4, textAlign: "center"}}>
                                    <Typography>
                                        {loadingMessages[messageIndex]}
                                    </Typography>
                                    <LinearProgress variant = "determinate" value = {progress}/>
                                </Box>)}
                    {error !== "" && (
                        <Alert severity="error" sx={{mb : 2}}>
                            {error}
                        </Alert>
                    )}
                    {analyze !== null && !loading && (
                        <Stack spacing ={2}>
                            <Typography>{analyze.summary}</Typography>
                            <Divider/>

                            {analyze.suggestions.filter(s => s.quantity > 0).length === 0 && (
                                <Typography color = "text.secondary">No suggestions for this cart.</Typography>
                            )}

                            {analyze.suggestions.filter((suggestion) => suggestion.quantity > 0).map((suggestion) => {
                                const decision = decisions[suggestion.productId]
                                return (
                                    <Box key = {suggestion.productId} sx = {{
                                        border: "1px solid",
                                        borderColor: "divider",
                                        borderRadius: 1,
                                        p: 2,
                                    }}>
                                        <Box
                                            sx = {{display: "flex", justifyContent: "space-between"}}
                                        >
                                            <Typography variant="h4">
                                                {suggestion.name} x {suggestion.quantity}
                                            </Typography>
                                            <Typography variant="subtitle1">
                                                {suggestion.priceLabel}
                                            </Typography>
                                        </Box>
                                        <Typography variant="body2" 
                                            color="text.secondary"
                                            sx = {{mt: 0.5}}>
                                            {suggestion.reason}
                                        </Typography>
                                        {suggestion.savingsLabel !== null && (
                                            <Typography variant="body2" 
                                            color="success"
                                            sx = {{mt: 0.5}}>
                                                Total savings: {suggestion.savingsLabel}
                                            </Typography>
                                        )}

                                        <Box sx = {{mt: 1.5}}>
                                            {decision === undefined ? (
                                                <Stack direction='row'>
                                                    <Button size = "small" variant="contained" startIcon = {<CheckIcon/>} onClick = {() => handleApprove(suggestion)}> Approve</Button>
                                                    <Button size = "small" variant="contained" startIcon = {<CloseIcon/>} onClick = {() => handleDecline(suggestion)}> Decline</Button>
                                                </Stack>
                                            ) : <Box/>
                                        }
                                        </Box>
                                    </Box>
                                )
                            })}
                        </Stack>
                    )}

                </DialogContent>
        </Dialog>
    )
}

export default AnalyzeDialog