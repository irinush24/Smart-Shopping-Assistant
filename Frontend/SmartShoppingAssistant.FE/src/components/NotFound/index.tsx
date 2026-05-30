import { Box } from "@mui/material";

const NotFound = () => {
    return (
        // Make a separate CSS class for this in the future

        // Middleware = prelucram datele si le dam mai departe, daca e nevoie la frontend (interceptam requestul, facem ceva cu el, apoi il trimitem mai departe)
        // useCors a fost middleware pe backend e de safety pentru a stabili cine poate accesa backend
        <Box sx={{ display: "flex", flexDirection: "column", alignItems: "center", justifyContent: "center", height: "80vh" }}>
        <Box>Oops! The page you're looking for doesn't exist.</Box>
    </Box>)
}

export default NotFound;