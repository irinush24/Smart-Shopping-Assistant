import {Box, Container, Typography} from "@mui/material"

function Home()
{
  return (
  <Container maxWidth="md" sx={{ mt: 8, mb: 8, textAlign:"center"}}>
    <Box sx={{justifyContent: "space-between"}}>
      <Typography variant="h1" gutterBottom>Handmade Hugs by Iri</Typography>
      <Typography variant="h5" sx ={{lineHeight: 1.8, color: "text.secondary"}}>
          Hi, I'm Iri! 🎀 I am the hands and heart behind the crochet hooks here at Handmade Hugs. I started this shop to share my love for all things cozy, hyper-cute, and nostalgic. Creating these little friends and delicate accessories brings me so much joy, and my biggest hope is that when you open your package, you can feel the love and care stitched into every single loop. Thank you for supporting my small handmade dream! ✨
      </Typography>
      <Box component = "img" 
        src="/public/tag.png" 
        alt="Handmade Hugs by Iri main tag"
        sx={{
          width: "100%",
          maxWidth: 400,
          height: "auto",
          borderRadius: "30px",
          border: "4px dashed #ffb3c6",
          boxShadow: "0 15px 30px rgba(255, 77, 109, 0.2)",
          mt: 2
        }}>
      </Box>
    </Box>
  </Container>
  );
}

export default Home;