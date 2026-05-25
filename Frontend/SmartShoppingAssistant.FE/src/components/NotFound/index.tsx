import { Box } from "@mui/material";

const NotFound = () => {
    return (
        <Box sx={{ display: "flex", flexDirection: "column", alignItems: "center", justifyContent: "center", height: "80vh" }}>
        <Box>Oops! The page you're looking for doesn't exist.</Box>
    </Box>)
}

export default NotFound;