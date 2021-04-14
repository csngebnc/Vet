import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { VaccineRecordDto } from 'src/app/_models/VaccineRecordDto';
import { VaccineService } from 'src/app/_services/vaccine.service';
import { AddVaccineRecordComponent } from '../add-vaccine-record/add-vaccine-record.component';
import { EditVaccineRecordComponent } from '../edit-vaccine-record/edit-vaccine-record.component';

@Component({
  selector: 'app-list-vaccine-records',
  templateUrl: './list-vaccine-records.component.html',
  styleUrls: ['./list-vaccine-records.component.css']
})
export class ListVaccineRecordsComponent implements OnInit {

  @Input() animalId;

  records: VaccineRecordDto[];

  dataSource;
  displayedColumns: string[] = ['name', 'date', 'button'];

  constructor(private vaccineService: VaccineService, private modalService: NgbModal, private ngbModal: NgbActiveModal) { }

  ngOnInit() {
    this.refreshRecords();
  }

  refreshRecords() {
    this.vaccineService.getVaccineRecordsByAnimal(this.animalId).subscribe((records: VaccineRecordDto[]) => {
      this.records = records;
      this.records.forEach(vr => vr.date = new Date(vr.date));
      this.dataSource = new MatTableDataSource<VaccineRecordDto>(this.records);
    })
  }

  open() {
    const modalRef = this.modalService.open(AddVaccineRecordComponent);
    modalRef.componentInstance.animalId = this.animalId;
    modalRef.result.then((record: VaccineRecordDto) => {
      record.date = new Date(record.date);
      this.records.push(record);
      this.dataSource = new MatTableDataSource<VaccineRecordDto>(this.records);
    }, () => { })
  }

  openEdit(id) {
    const modalRef = this.modalService.open(EditVaccineRecordComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then((record: VaccineRecordDto) => {
      record.date = new Date(record.date);
      this.records[this.records.map((r) => { return r.id }).indexOf(record.id)] = record;
      this.dataSource = new MatTableDataSource<VaccineRecordDto>(this.records);
    }, () => { })
  }

  deleteRecord(id) {
    this.vaccineService.deleteVaccineRecord(id).subscribe((res: boolean) => {
      if (res) {
        this.records = this.records.filter(r => r.id != id);
        this.dataSource = new MatTableDataSource<VaccineRecordDto>(this.records);
      } else {
        alert('A rekord törlése nem sikerült.');
      }
    })
  }
}
