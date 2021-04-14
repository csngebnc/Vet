import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TreatmentTimeDto } from 'src/app/_models/treatmenttimedto';
import { TreatmenttimeService } from 'src/app/_services/treatmenttime.service';
import { AddTreatmentTimeComponent } from '../add-treatment-time/add-treatment-time.component';
import { EditTreatmentTimeComponent } from '../edit-treatment-time/edit-treatment-time.component';

@Component({
  selector: 'app-list-treatment-time',
  templateUrl: './list-treatment-time.component.html',
  styleUrls: ['./list-treatment-time.component.css']
})
export class ListTreatmentTimeComponent implements OnInit {

  days: string[] = ["vasárnap", "hétfő", "kedd", "szerda", "csütörtök", "péntek", "szombat"];
  dataSource;
  displayedColumns: string[] = ['id', 'day', 'time', 'duration', 'status', 'button'];

  treatmentTimes: TreatmentTimeDto[] = [];
  constructor(private treatmentTimeService: TreatmenttimeService, private route: ActivatedRoute, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.treatmentTimeService.getTreatmentTimeByTreatmentId(this.route.snapshot.paramMap.get('treatmentid'))
      .subscribe((treatmentTimes: TreatmentTimeDto[]) => {
        this.treatmentTimes = treatmentTimes;
        this.dataSource = new MatTableDataSource<TreatmentTimeDto>(this.treatmentTimes);
      })

  }

  open() {
    const modalRef = this.modalService.open(AddTreatmentTimeComponent, { size: 'lg' });
    modalRef.componentInstance.treatmentId = this.route.snapshot.paramMap.get('treatmentid');
    modalRef.result.then((treatmentTime: TreatmentTimeDto) => {
      this.treatmentTimes.push(treatmentTime);
      this.dataSource = new MatTableDataSource<TreatmentTimeDto>(this.treatmentTimes);
    }, () => { })
  }

  openEdit(id) {
    const modalRef = this.modalService.open(EditTreatmentTimeComponent, { size: 'lg' });
    modalRef.componentInstance.id = id;
    modalRef.result.then((treatmentTime: TreatmentTimeDto) => {
      this.treatmentTimes[this.treatmentTimes.map((tt) => { return tt.id }).indexOf(id)] = treatmentTime;
      this.dataSource = new MatTableDataSource<TreatmentTimeDto>(this.treatmentTimes);
    }, () => { });
  }

  deleteTreatmentTime(id: number) {
    this.treatmentTimeService.deleteTreatmentTime(id).subscribe(() => {
      this.treatmentTimes = this.treatmentTimes.filter(tt => tt.id !== id)
      this.dataSource = new MatTableDataSource<TreatmentTimeDto>(this.treatmentTimes);
    });
  }

  changeStateOfTreatmentTime(id) {
    this.treatmentTimeService.changeState(id).subscribe(() => {
      this.treatmentTimes[this.treatmentTimes.map(tt => tt.id).indexOf(id)].isInactive = !this.treatmentTimes[this.treatmentTimes.map(tt => tt.id).indexOf(id)].isInactive;
      this.dataSource = new MatTableDataSource<TreatmentTimeDto>(this.treatmentTimes);
    });
  }
}
