import {Alert, Box, CircularProgress, Container, IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Tooltip} from "@mui/material"
import {useEffect, useState} from "react"
import type { Product } from "../shared/types/Product";
import { productsApi } from "../../api/clients/ProductApiClient";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import PageHeader from "../common/PageHeader";
import ConfirmDialog from "../common/ConfirmDialog";
import ProductFormDialog from "../Products/ProductFormDialog";

function Products()
{
    const [products, setProducts] = useState<Product[]>([]);     // List of products shown in the table
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const [formOpen, setFormOpen] = useState(false);
    const [editing, setEditing] = useState<Product | null>(null);

    const [deleting, setDeleting] = useState<Product | null>(null);
    const [confirmOpen, setConfirmOpen] = useState(false);

    function loadProducts() {
        productsApi
            .getAll()
            .then((data) => {
                setProducts(data);
                setError("");
            })
            .catch((err) => setError((err as Error).message))
            .finally(() => setLoading(false));
    }

    function handleAdd() {
        setEditing(null);
        setFormOpen(true);
    }

    function handleEdit(product: Product) {
        setEditing(product);
        setFormOpen(true);
    }

    function handleDeleteClick(product: Product) {
        setDeleting(product);
        setConfirmOpen(true);
    }

    async function handleDelete() {
        if (deleting === null) return;
        setConfirmOpen(false);
        try {
            await productsApi.remove(deleting.id);
            loadProducts();
        } catch (err) {
            setError((err as Error).message);
        }
    }

    useEffect(() => {
        loadProducts();
    }, []);

    return (
        <Container maxWidth="xl" sx={{ mt: 4, mb: 4 }}>
            <PageHeader
                title={"Products"}
                actionLabel={"Add Product"}
                onAction={handleAdd}
            />
            { error !== "" && (
                <Alert severity="error" sx = {{mb : 2}}>
                    {error}
                </Alert>
            )}

            {loading ? (
                <Box sx={{display: "flex", justifyContent: "center" , mt: 4}}>
                    <CircularProgress />
                </Box>
            ) : (
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell>Price</TableCell>
                                <TableCell>Description</TableCell>
                                <TableCell align="right">Actions</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {products.map((product) => (
                                <TableRow key={product.id}>
                                    <TableCell>{product.name}</TableCell>
                                    <TableCell>${product.price.toFixed(2)}</TableCell>
                                    <TableCell>{product.description}</TableCell>
                                    
                                    <TableCell align="right">
                                        <Tooltip title="Edit">
                                            <IconButton
                                                onClick={() => handleEdit(product)}
                                                color="primary"
                                            >
                                                <EditIcon/>
                                            </IconButton>
                                        </Tooltip>
                                        <Tooltip title="Delete">
                                            <IconButton
                                                onClick={() => handleDeleteClick(product)}
                                                color="error"
                                            >
                                                <DeleteIcon/>
                                            </IconButton>
                                        </Tooltip>
                                    </TableCell>
                                </TableRow>
                            ))}
                            {products.length === 0 && (
                            <TableRow>
                                <TableCell colSpan={4} align="center">
                                    No products have been added yet.
                                </TableCell>
                            </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
            )}
            {formOpen && (
                <ProductFormDialog
                    product={editing}
                    onClose={() => setFormOpen(false)}
                    onSaved={() => {
                        setFormOpen(false);
                        loadProducts();
                    }}
                />
            )}
            <ConfirmDialog
                open={confirmOpen}
                title="Delete product"
                description={`Are you sure you want to delete "${deleting?.name}"?`}
                confirmLabel="Delete"
                onConfirm={handleDelete}
                onCancel={() => setConfirmOpen(false)}
            />
        </Container>
    );
}

export default Products;