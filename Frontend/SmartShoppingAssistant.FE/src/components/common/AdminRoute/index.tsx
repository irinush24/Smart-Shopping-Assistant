import {Navigate, Outlet} from 'react-router-dom'

function AdminRoute()
{
    // Check the role saved in the NavBar
    const currentRole = localStorage.getItem('userRole')

    // Redirect to homepage if they do not have the admin role
    if(currentRole !== 'admin')
        return <Navigate to='*' replace />  // ensures the end user can't just click the browser's "Back" button to try again
    
    // If the end user is an admin, render the child route they requested
    return <Outlet/>
}

export default AdminRoute;