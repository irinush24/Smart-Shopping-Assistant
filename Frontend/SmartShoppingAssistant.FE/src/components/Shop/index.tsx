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
    Divider,
    InputLabel,
    FormControl,
    MenuItem,
    Select,
    TextField,
    Typography,
    FormGroup,
    FormControlLabel,
    Slider,
    Switch
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
    const [priceRange, setPriceRange] = useState<[number, number]>([0, 100]);
    const [hasImageFilter, setHasImageFilter] = useState(false);

    const minPrice = useMemo(() =>
    {
        if(products.length === 0) return 0;
        return Math.min(...products.map(p => p.price));
    }, [products]);

    const maxPrice = useMemo(() => 
    {
        if(products.length === 0) return 10000;
        return Math.max(...products.map(p => p.price));
    }, [products]);

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
        })
    }

    const handleSliderChange = (newValue: number | number[]) => {
        priceRange[0] = (newValue as number[])[0];
        priceRange[1] = (newValue as number[])[1];
        setPriceRange([...priceRange]);
    }

    const handleImage = () => {
        setHasImageFilter((prev : boolean) => !prev);
    }

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

            const matchesPrice = product.price >= priceRange[0] && product.price <= priceRange[1];

            const matchesImage = hasImageFilter ? Boolean(product.imageUrl) : true;

            return matchesCategory && matchesSearch && matchesPrice && matchesImage;            
        });
    }, [products, search, selectedCategories, priceRange, hasImageFilter]);


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
                if(data.length > 0)
                    setPriceRange([minPrice, maxPrice]);
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
                <Typography variant="h2">Shop</Typography>
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
                sx={{ mb: 2, width: "75%",
                    "& .MuiOutlinedInput-root":{
                        bgcolor: "background.paper",
                        borderRadius: "50px"
                    }
                }}
                />

                <FormControl sx = {{mb:2, width: "23%"}}>
                    <InputLabel id = "sort-select-label">Sort by:</InputLabel>
                    <Select
                        labelId="sort-select-label"
                        id="sort-select"
                        label="Sort by:"
                        value={sort}
                        onChange={(e) => setSort(e.target.value)}
                        sx={{bgcolor: "background.paper", borderRadius:"50px"}}
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
                        <Typography variant="h4" sx = {{mb: 1}}>Filters</Typography>

                        <Divider variant = "fullWidth" sx = {{ mb: 2, mt: 1}}></Divider>
                        <Typography variant="h6" sx = {{mb: 1}}>Categories</Typography>
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

                        <Divider variant = "fullWidth" sx = {{ mb: 2, mt: 1}}></Divider>
                        <Typography variant="h6" sx = {{mb: 5}}>Price Range</Typography>
                        <Slider 
                            getAriaLabel={() => 'Price range'}
                            value = {priceRange}
                            onChange={(_, newValue) => handleSliderChange(newValue as number[])}
                            valueLabelDisplay="on"
                            min = {minPrice}
                            max = {maxPrice}
                        />

                        <Divider variant = "fullWidth" sx = {{mb: 2, mt: 1}}></Divider>
                        <Typography variant="h6" sx = {{mb:1}}>Image</Typography>
                        <FormControl>
                            <FormControlLabel control = {
                                <Switch 
                                    checked = {hasImageFilter}
                                    onChange = {handleImage}
                                />} 
                            label = "Has image"/>
                        </FormControl>
                    </Box> 

                    {/* Products grid positioned on the right*/}
                    <Box 
                        sx = {{
                            flexGrow: 1,
                            maxHeight: "100vh",
                            overflowY: "auto",
                            pr: 1,
                            "&::-webkit-scrollbar": {
                                    width: "10px",
                                },

                                "&::-webkit-scrollbar-track": {
                                    backgroundColor: "#fff0f3",
                                    borderRadius: "10px",
                                },
                                "&::-webkit-scrollbar-thumb": {
                                    backgroundColor: "ffb3c6",
                                    borderRadius: "10px",
                                },
                                "&::-webkit-scrollbar-thumb-hover": {
                                    backgroundColor: "#ff4d6d",
                                }
                        }}>
                        <Box 
                            sx={{
                                display: 'grid',
                                gap: 2,
                                alignContent: 'start',
                                gridTemplateColumns: 'repeat(auto-fill, minmax(240px, 1fr))',                                
                            }}>
                            {sortedProducts.map((product) => (
                                <Card
                                    key={product.id}
                                    sx={{display: 'flex', flexDirection: 'column', height: "100%"}}
                                >
                                    <CardMedia
                                        component="img"
                                        height="280"
                                        image={product.imageUrl}
                                        alt={product.name}
                                        sx={{ objectFit: 'cover', objectPosition: "top center", flexShrink: "0"}}
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
                </Box>
            )}
        </Container>
    );
}

export default Shop;