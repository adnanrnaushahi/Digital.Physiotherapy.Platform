import React, { useState, useEffect } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    TextField,
    Box,
    Chip,
    SelectChangeEvent
} from '@mui/material';
import { Patient } from '../models/Patient';
import { Exercise } from '../models/Exercise';
import { PrescriptionCreate } from '../models/Prescription';

interface PrescriptionFormProps {
    open: boolean;
    onClose: () => void;
    onSubmit: (prescription: PrescriptionCreate) => void;
    patients: Patient[];
    exercises: Exercise[];
}

const PrescriptionForm: React.FC<PrescriptionFormProps> = ({
    open,
    onClose,
    onSubmit,
    patients,
    exercises
}) => {
    const [formData, setFormData] = useState<PrescriptionCreate>({
        patientId: 0,
        exerciseIds: [],
        notes: ''
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handlePatientChange = (e: SelectChangeEvent<number>) => {
        setFormData(prev => ({
            ...prev,
            patientId: e.target.value as number
        }));
    };

    const handleExerciseChange = (e: SelectChangeEvent<number[]>) => {
        const selectedExerciseIds = e.target.value as number[];
        setFormData(prev => ({
            ...prev,
            exerciseIds: selectedExerciseIds
        }));
    };

    const handleSubmit = () => {
        onSubmit(formData);
        onClose();
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth>
            <DialogTitle>Create New Prescription</DialogTitle>
            <DialogContent>
                <Box component="form" sx={{ mt: 2 }}>
                    <FormControl fullWidth margin="normal">
                        <InputLabel id="patient-select-label">Patient</InputLabel>
                        <Select
                            labelId="patient-select-label"
                            id="patient-select"
                            value={formData.patientId}
                            label="Patient"
                            onChange={handlePatientChange}
                        >
                            {patients.map(patient => (
                                <MenuItem key={patient.id} value={patient.id}>
                                    {patient.name}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>

                    <FormControl fullWidth margin="normal">
                        <InputLabel id="exercise-select-label">Exercises</InputLabel>
                        <Select
                            labelId="exercise-select-label"
                            id="exercise-select"
                            multiple
                            value={formData.exerciseIds}
                            label="Exercises"
                            onChange={handleExerciseChange}
                            renderValue={(selected) => (
                                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                                    {(selected as number[]).map((value) => {
                                        const exercise = exercises.find(ex => ex.id === value);
                                        return (
                                            <Chip key={value} label={exercise ? exercise.name : value} />
                                        );
                                    })}
                                </Box>
                            )}
                        >
                            {exercises.map(exercise => (
                                <MenuItem key={exercise.id} value={exercise.id}>
                                    {exercise.name}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>

                    <TextField
                        fullWidth
                        margin="normal"
                        name="notes"
                        label="Notes"
                        multiline
                        rows={4}
                        value={formData.notes}
                        onChange={handleChange}
                    />
                </Box>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose}>Cancel</Button>
                <Button
                    onClick={handleSubmit}
                    variant="contained"
                    color="primary"
                    disabled={formData.patientId === 0 || formData.exerciseIds.length === 0}
                >
                    Create
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default PrescriptionForm;