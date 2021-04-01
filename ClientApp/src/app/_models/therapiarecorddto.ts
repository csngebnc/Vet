export interface TherapiaRecordDto {
    id: number;
    medicalRecordId: number;
    therapiaId: number;
    therapiaName: string;
    therapiaUnit: string;
    amount: number;
}

export interface LocalTherapiaRecord {
    id: number,
    dbId?: number
    therapiaName: string,
    therapiaId: number,
    amount: number
}

export interface TherapiaOnMedicalRecord {
    therapiaId: number;
    amount: number;
}