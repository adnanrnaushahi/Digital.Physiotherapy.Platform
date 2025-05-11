import React, { useState, useEffect } from 'react';
import {
    Paper,
    Typography,
    List,
    ListItem,
    ListItemText,
    Button,
    Box,
    Chip,
    CircularProgress,
    Divider
} from '@mui/material';
import { Prescription } from '../models/Prescription';
import { Patient } from '../models/Patient';
import { Exercise } from '../models/Exercise';
import PrescriptionForm from './PrescriptionForm';
import {
    getPrescriptions,
    getPatients,
    getExercises,
    createPrescription
} from '../api/api';

const PrescriptionList: React.FC = () => {
    const [prescriptions, setPrescriptions] = useState<Prescription[]>([]);
    const [patients, setPatients] = useState<Patient[]>([]);
    const [exercises, setExercises] = useState<Exercise[]>([]);
    const [loading, setLoading] = useState(true);
    const [dialogOpen, setDialogOpen] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadData = async () => {
            try {
                setLoading(true);
                setError(null);

                const [prescriptionsData, patientsData, exercisesData] = await Promise.all([
                    getPrescriptions(),
                    getPatients(),
                    getExercises()
                ]);

                setPrescriptions(prescriptionsData);
                setPatients(patientsData);
                setExercises(exercisesData);
            } catch (err) {
                console.error('Error loading data:', err);
                setError('Failed to load prescriptions. Please try again later.');
            } finally {
                setLoading(false);
            }
        };

        loadData();
    }, []);

    const handleCreatePrescription = async (prescriptionData: any) => {
        try {
            setError(null);
            const newPrescription = await createPrescription(prescriptionData);
            setPrescriptions(prev => [...prev, newPrescription]);
        } catch (err) {
            console.error('Error creating prescription:', err);
            setError('Failed to create prescription. Please try again.');
        }
    };

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" m={4}>
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Paper sx={{ p: 2, m: 2 }}>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="h5">Exercise Prescriptions</Typography>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => setDialogOpen(true)}
                >
                    New Prescription
                </Button>
            </Box>

            {error && (
                <Typography color="error" sx={{ mb: 2 }}>
                    {error}
                </Typography>
            )}

            <List>
                {prescriptions.length > 0 ? (
                    prescriptions.map((prescription) => (
                        <ListItem key={prescription.id} divider>
                            <ListItemText
                                primary={
                                    <Typography variant="h6">
                                        Patient: {prescription.patientName}
                                    </Typography>
                                }
                                secondary={
                                    <Box mt={1}>
                                        <Typography variant="body2" color="text.primary">
                                            Created: {new Date(prescription.createdDate).toLocaleDateString()}
                                        </Typography>

                                        <Divider sx={{ my: 1 }} />

                                        <Typography variant="body2" component="div" sx={{ mb: 1 }}>
                                            Exercises:
                                        </Typography>
                                        <Box display="flex" flexWrap="wrap" gap={1} mb={1}>
                                            {prescription.exercises.map(ex => (
                                                <Chip
                                                    key={ex.id}
                                                    label={`${ex.name} (${ex.sets} x ${ex.repetitionCount})`}
                                                    size="small"
                                                    color="primary"
                                                    variant="outlined"
                                                />
                                            ))}
                                        </Box>

                                        {prescription.notes && (
                                            <>
                                                <Typography variant="body2" component="div" sx={{ mt: 1 }}>
                                                    Notes:
                                                </Typography>
                                                <Typography variant="body2" sx={{ ml: 1 }}>
                                                    {prescription.notes}
                                                </Typography>
                                            </>
                                        )}
                                    </Box>
                                }
                            />
                        </ListItem>
                    ))
                ) : (
                    <Typography variant="body1" sx={{ textAlign: 'center', py: 3 }}>
                        No prescriptions found. Create your first prescription!
                    </Typography>
                )}
            </List>

            <PrescriptionForm
                open={dialogOpen}
                onClose={() => setDialogOpen(false)}
                onSubmit={handleCreatePrescription}
                patients={patients}
                exercises={exercises}
            />
        </Paper>
    );
};

export default PrescriptionList;