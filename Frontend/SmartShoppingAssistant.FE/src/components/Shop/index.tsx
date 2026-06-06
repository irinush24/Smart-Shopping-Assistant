import {
    Alert,
    Box,
    Button,
    Card,
    CardActions,
    CardContent,
    CardMedia,
    CircularProgress,
    Container,
    Checkbox,
    InputLabel,
    FormControl,
    MenuItem,
    Select,
    TextField,
    Typography,
    FormGroup,
    FormControlLabel,
} from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import {useCart} from "../../context/CartContext/cart-context"
import { productsApi } from "../../api/clients/ProductApiClient";
import { categoriesApi } from "../../api/clients/CategoryApiClient";
import type { Product } from "../shared/types/Product";
import type { Category } from "../shared/types/Category";
import AddShoppingCartIcon from '@mui/icons-material/AddShoppingCart'

function Shop() {
    const [products, setProducts] = useState<Product[]>([]);
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    // UI States
    const [search, setSearch] = useState("");
    const [sort, setSort] = useState("A-to-Z");
    const [selectedCategories, setSelectedCategories] = useState<string[]>([]);

    const {addItem} = useCart();

    const handleAddToCart = async (product : Product) => {
        await addItem(product.id, 1)
    }

    const handleCategoryToggle = (categoryId: string) => {
        setSelectedCategories((prev) => {
            if (prev.includes(categoryId)) {
                return prev.filter((id) => id !== categoryId);
            } else {
                return [...prev, categoryId];
            }
        });
    };

    // Pipelining for the products

    // Step 1: Filter products based on search and category
    const filteredProducts = useMemo(() => {
        return products.filter((product) => {
            // Search filter
            const matchesSearch = product.name
                .toLocaleLowerCase()
                .includes(search.trim().toLocaleLowerCase());

            // Category filter
            const matchesCategory = selectedCategories.length === 0 || selectedCategories.some((selectedName) => product.categories.includes(selectedName));
            return matchesCategory && matchesSearch;
        });
    }, [products, search, selectedCategories]);


    // Step 2: Sort the filtered products
    const sortedProducts = useMemo(() => {
        const sorted = [...filteredProducts];
        switch (sort) {
            case "A-to-Z":
                sorted.sort((a, b) => a.name.localeCompare(b.name));
                break;
            case "Z-to-A":
                sorted.sort((a, b) => b.name.localeCompare(a.name));
                break;
            case "PriceLowHigh":
                sorted.sort((a, b) => a.price - b.price);
                break;
            case "PriceHighLow":
                sorted.sort((a, b) => b.price - a.price);
                break;
        }
        return sorted;
    }, [filteredProducts, sort]);

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

    function loadCategories() {
        categoriesApi
            .getAll()
            .then((data) => {
                setCategories(data);
                setError("");
            })
            .catch((err) => setError((err as Error).message))
            .finally(() => setLoading(false));
    }

    useEffect(() => {
        loadProducts();
        loadCategories();
    }, []);

    return (
        <Container maxWidth="xl" sx={{ py: 4 }}>
            {error !== "" && (
                <Alert severity="error" sx={{ mb: 2 }}>
                    {error}
                </Alert>
            )}
            <Box
                sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    mb: 2,
                }}
            >
                <Typography variant="h4">Shop</Typography>
            </Box>
            <Box
                sx={{
                    display: "flex",
                    justifyContent: "space-between",
                }}
            >
                <TextField
                label="Search products"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                sx={{ mb: 2, width: "75%"}}
                />

                <FormControl sx = {{mb:2, width: "23%"}}>
                    <InputLabel id = "sort-select-label">Sort by:</InputLabel>
                    <Select
                        labelId="sort-select-label"
                        id="sort-select"
                        label="Sort by:"
                        value={sort}
                        onChange={(e) => setSort(e.target.value)}
                    >
                        <MenuItem value={"A-to-Z"}>A-Z</MenuItem>
                        <MenuItem value={"Z-to-A"}>Z-A</MenuItem>
                        <MenuItem value={"PriceLowHigh"}>Price: Ascending</MenuItem>
                        <MenuItem value={"PriceHighLow"}>Price: Descending</MenuItem>
                    </Select>
                </FormControl>
            </Box>
            {loading ? (
                <Box sx={{ display: "flex", justifyContent: "center", mt: 4 }}>
                    <CircularProgress />
                </Box>
            ) : (
                <Box
                    sx={{
                        display: 'flex',
                        gap: 2,
                        flexDirection: 'row',
                    }}>
                    <Box sx = {{width: '250px', flexShrink: 0}}>

                        {/* Filters sidebar positioned on the left*/}
                        <Typography variant="h6" sx = {{mb: 1}}>Filters</Typography>
                        <FormGroup>
                            {categories.map((category) => (
                                <FormControlLabel
                                    key={category.id}
                                    label={category.name}
                                    control={
                                        <Checkbox
                                            checked={selectedCategories.includes(category.name)}
                                            onChange={() => handleCategoryToggle(category.name)}
                                        />
                                    }
                                />
                            ))}
                        </FormGroup>
                    </Box>
                    

                    {/* Products grid positioned on the right*/}
                    <Box 
                        sx={{
                            display: 'grid',
                            gap: 2,
                            flexGrow: 1,
                            alignContent: 'start',
                            gridTemplateColumns: 'repeat(auto-fill, minmax(240px, 1fr))'
                        }}>
                        {sortedProducts.map((product) => (
                            <Card
                                key={product.id}
                                sx={{display: 'flex', flexDirection: 'column'}}
                            >
                                <CardMedia
                                    component="img"
                                    height="160"
                                    image={product.imageUrl}
                                    alt={product.name}
                                    sx={{ objectFit: 'cover' }}
                                />
                                <CardContent sx={{ flexGrow: 1 }}>
                                    <Typography variant="h6">{product.name}</Typography>
                                    <Typography variant="body2" color="textSecondary">{product.description}</Typography>
                                    <Typography variant="body1" sx= {{ pt: 1 }}>{product.priceLabel}</Typography>
                                </CardContent>
                                <CardActions>
                                    <Button
                                        fullWidth
                                        variant="contained"
                                        startIcon={<AddShoppingCartIcon />}
                                        onClick={() => handleAddToCart(product)}
                                    >
                                        Add to Cart
                                    </Button>
                                </CardActions>
                            </Card>
                        ))}
                        {sortedProducts.length === 0 && (
                            <Typography>No products found.</Typography>
                        )}
                    </Box>
                </Box>
            )}
        </Container>
    );
}

export default Shop;