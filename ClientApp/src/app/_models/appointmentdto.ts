export interface AppointmentDto{
    id: Number,
    startDate: Date,
    endDate: Date,
    treatmentName: string,
    doctorName: string,
    ownerId: string,
    ownerName: string,
    animalId: number,
    animalName: string,
    details: string;
    isResigned: boolean
}
