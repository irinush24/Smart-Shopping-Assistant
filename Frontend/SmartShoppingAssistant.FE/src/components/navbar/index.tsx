import { AppBar, Toolbar, Button, Box } from "@mui/material"
import { NavLink, Link } from "react-router-dom";
import logo from "../../assets/logo.png"

function NavBar()
{
    // const nav = useNavigate() another option (a hook) can be used with conditions
    // use in the name => hook
    return <AppBar position="static" elevation={0}>
            <Toolbar>
                <Link to = "/">
                    <Box
                        component = "img"
                        src = {logo}
                        alt = "Smart Shopping Assistant Logo"
                        sx = {{ height : 56, mr: 2}}  // inline styling de la MUI
                    />
                </Link>
                <Button component = {NavLink} to="/" variant = "text" color="inherit">Home</Button>
                <Button component = {NavLink} to="/categories" variant = "text" color="inherit">Categories</Button>
                <Button component = {NavLink} to="/products" variant = "text" color="inherit">Products</Button>
                <Button component = {NavLink} to="/promotions" variant = "text" color="inherit">Promotions</Button>
            </Toolbar>
        </AppBar>
}

export default NavBar;