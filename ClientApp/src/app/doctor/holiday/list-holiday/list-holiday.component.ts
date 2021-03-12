import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HolidayDto } from 'src/app/_models/holidaydto';
import { DoctorService } from 'src/app/_services/doctor.service';
import { AddHolidayComponent } from '../add-holiday/add-holiday.component';
import { EditHolidayComponent } from '../edit-holiday/edit-holiday.component';

@Component({
  selector: 'app-list-holiday',
  templateUrl: './list-holiday.component.html',
  styleUrls: ['./list-holiday.component.css']
})
export class ListHolidayComponent implements OnInit {

  holidays: HolidayDto[] = [];

  dataSource;
  displayedColumns: string[] = ['firstDate', 'lastDate', 'button'];

  constructor(private doctorService: DoctorService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.refreshHolidays()
  }

  refreshHolidays() {
    this.doctorService.getOwnHolidays().subscribe((holidays: HolidayDto[]) => {
      this.holidays = holidays;
      this.holidays.forEach(h => {
        h.startDate = new Date(h.startDate);
        h.endDate = new Date(h.endDate);
      });
      this.refreshSource();
    })
  }

  open() {
    const modalRef = this.modalService.open(AddHolidayComponent);
    modalRef.result.then(() => {
      this.refreshHolidays();
    }, () => { })
  }

  openEdit(id) {
    const modalRef = this.modalService.open(EditHolidayComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then(() => {
      this.refreshHolidays();
    }, () => { })
  }

  delete(id) {
    this.doctorService.deleteHoliday(id).subscribe(success => {
      if (success) {
        this.holidays = this.holidays.filter(h => h.id !== id);
        this.refreshSource();
      }
    })
  }

  refreshSource() {
    this.dataSource = new MatTableDataSource<HolidayDto>(this.holidays);
  }

}
