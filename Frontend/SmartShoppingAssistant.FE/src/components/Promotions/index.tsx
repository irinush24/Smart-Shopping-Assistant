import {Alert, Box, CircularProgress, Container, IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Tooltip} from "@mui/material"
import {useEffect, useState} from "react"
import type {Promotion} from "../shared/types/Promotion"
import { promotionsApi } from "../../api/clients/PromotionApiClient";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import PageHeader from "../common/PageHeader";
import PromotionFormDialog from "./PromotionFormDialog";
import ConfirmDialog from "../common/ConfirmDialog";


function Promotions()
{
    const [promotions, setPromotions] = useState<Promotion[]>([]);      // List of promotions shown inside the table
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const [formOpen, setFormOpen] = useState(false);
    const [editing, setEditing] = useState<Promotion | null> (null);
    const [deleting, setDeleting] = useState<Promotion | null>(null);
    const [confirmOpen, setConfirmOpen] = useState(false);

    function loadPromotions() {
        promotionsApi
        .getAll()
        .then((data) => {
            setPromotions(data);
            setError("");
        })
        .catch((err) => setError((err as Error).message))
        .finally(() => setLoading(false));
    }

    function handleAdd() {
        setEditing(null);
        setFormOpen(true);
    }

    function handleEdit(promotion : Promotion) {
        setDeleting(promotion);
        setConfirmOpen(true);
    }

    function handleDeleteClick(promotion : Promotion) {
        setDeleting(promotion);
        setConfirmOpen(true);
    }

    async function handleDelete() {
        if(deleting == null)    return;
        setConfirmOpen(false);
        try
        {
            await promotionsApi.remove(deleting.id);
            loadPromotions();
        }
        catch (err)
        {
            setError((err as Error).message);
        }
    }

    useEffect(() => {
        loadPromotions();
    }, []);

    return (
        <Container maxWidth="xl" sx={{ py: 4 }}>
      <PageHeader
        title={"Promotions"}
        actionLabel={"Add Promotion"}
        onAction={handleAdd}
      />

      {error !== "" && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      {loading ? (
        <Box sx={{ display: "flex", justifyContent: "center", mt: 4 }}>
          <CircularProgress />
        </Box>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Name</TableCell>
                <TableCell>Type</TableCell>
                <TableCell>Reward Type</TableCell>
                <TableCell>Threshold</TableCell>
                <TableCell>Reward Value</TableCell>
                <TableCell>Status</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {promotions.map((promotion) => (
                <TableRow key={promotion.id} hover>
                  <TableCell>{promotion.name}</TableCell>
                  <TableCell>{String(promotion.promotionType) === "BuyXGetY" ? "Buy X Get Y" : "Percentage"}</TableCell>
                  <TableCell>{String(promotion.promotionReward) === "Quantity" ? "Quantity" : "Percentage"}</TableCell>
                  <TableCell>{promotion.threshold}</TableCell>
                  <TableCell>{promotion.rewardValue}</TableCell>
                  <TableCell>{String(promotion.isActive) === "true" ? "Active" : "Inactive"}</TableCell>
                  
                  <TableCell align="right">
                    <Tooltip title="Edit">
                      <IconButton
                        color="primary"
                        onClick={() => handleEdit(promotion)}
                      >
                        <EditIcon />
                      </IconButton>
                    </Tooltip>
                    <Tooltip title="Delete">
                      <IconButton
                        color="error"
                        onClick={() => handleDeleteClick(promotion)}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </Tooltip>
                  </TableCell>
                </TableRow>
              ))}
              {promotions.length === 0 && (
                <TableRow>
                  <TableCell colSpan={7} align="center">
                    No promotions have been added yet.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      )}
      {formOpen && (
        <PromotionFormDialog
          promotion={editing}
          onClose={() => setFormOpen(false)}
          onSaved={() => {
            setFormOpen(false);
            loadPromotions();
          }}
        />
      )}
      <ConfirmDialog
        open={confirmOpen}
        title="Delete category"
        description={`Are you sure you want to delete "${deleting?.name}"?`}
        confirmLabel="Delete"
        onConfirm={handleDelete}
        onCancel={() => setConfirmOpen(false)}
      />
    </Container>
    );
}

export default Promotions;