import { Box, Button, Typography } from "@mui/material";
import { Add } from "@mui/icons-material";

interface PageHeaderProps {
    title: string;
    actionLabel: string;
    onAction: () => void;
}

function PageHeader({title, actionLabel, onAction}: PageHeaderProps)
{
    return (
        <Box sx = {{display : "flex", justifyContent : "space-between", mb: 2}}>
            <Typography variant="h4">{title}</Typography>
            <Button variant = "contained" onClick = {onAction} startIcon = {<Add />}>
                {actionLabel}
            </Button>
        </Box>
    );
}

export default PageHeader;