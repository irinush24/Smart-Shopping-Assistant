import { AppBar, Toolbar, Button, Box } from "@mui/material"
import { NavLink, Link } from "react-router-dom";
import logo from "../../assets/logo.png"
// import './NavBar.css';

function NavBar()
{
    // const nav = useNavigate() another option (a hook) can be used with conditions
    // use in the name => hook
    return <AppBar position="static" color="default">
            <Toolbar>
                <Button component = {NavLink} to="/" variant = "contained">Home</Button>
                <Button component = {NavLink} to="/categories" variant = "contained">Categories</Button>
                <Button component = {NavLink} to="/products" variant = "contained">Products</Button>
                <Link to = "/">
                    <Box
                        component = "img"
                        src = {logo}
                        alt = "Smart Shopping Assistant Logo"
                        sx = {{ height : 56, mr: 2}}  // inline styling de la MUI
                    />
                </Link>
            </Toolbar>
        </AppBar>
}

export default NavBar;