import type { Exercise } from './Exercise';

export interface Prescription {
    id: number;
    patientId: number;
    patientName: string;
    createdDate: string;
    exercises: Exercise[];
    notes: string;
}

export interface PrescriptionCreate {
    patientId: number;
    exerciseIds: number[];
    notes: string;
}