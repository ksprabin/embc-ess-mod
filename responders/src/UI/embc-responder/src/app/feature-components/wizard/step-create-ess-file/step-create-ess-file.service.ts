import { Injectable } from '@angular/core';
import {
  InsuranceOption,
  NeedsAssessment
} from 'src/app/core/models/evacuation-file';
import { TabModel, WizardTabModelValues } from 'src/app/core/models/tab.model';
import * as globalConst from '../../../core/services/global-constants';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from 'src/app/shared/components/dialog/dialog.component';
import { InformationDialogComponent } from 'src/app/shared/components/dialog-components/information-dialog/information-dialog.component';
import { Subject } from 'rxjs';
import { AddressModel } from 'src/app/core/models/Address.model';
import { Address, PersonDetails } from 'src/app/core/api/models';
import { FormArray, FormControl, FormGroup } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class StepCreateEssFileService {
  private essTabs: Array<TabModel> = WizardTabModelValues.essFileTabs;

  private setNextTabUpdate: Subject<void> = new Subject();

  private paperESSFile: string;
  private evacuatedFromPrimary: boolean;
  private evacAddress: AddressModel;
  private facilityName: string;
  private insurance: InsuranceOption;
  private householdAffected: string;
  private emergencySupportServices: string;
  private referredServices: boolean;
  private referredServiceDetails: string[] = [];
  private externalServices: string;

  private haveHouseholdMembers: boolean;
  private householdMembers: PersonDetails[] = [];
  private householdMember: null | PersonDetails;
  private haveSpecialDiet: boolean;
  private specialDietDetails: null | string;
  private haveMedication: boolean;
  private medicationSupply: null | boolean;
  private sameLastNameCheck: null | boolean;

  constructor(private dialog: MatDialog) {}

  public get paperESSFiles(): string {
    return this.paperESSFile;
  }
  public set paperESSFiles(paperESSFile: string) {
    this.paperESSFile = paperESSFile;
  }

  public get evacuatedFromPrimaryAddress(): boolean {
    return this.evacuatedFromPrimary;
  }
  public set evacuatedFromPrimaryAddress(evacuatedFromPrimary: boolean) {
    this.evacuatedFromPrimary = evacuatedFromPrimary;
  }

  public get evacAddresS(): AddressModel {
    return this.evacAddress;
  }
  public set evacAddresS(evacAddress: AddressModel) {
    this.evacAddress = evacAddress;
  }

  public get facilityNames(): string {
    return this.facilityName;
  }
  public set facilityNames(facilityName: string) {
    this.facilityName = facilityName;
  }

  public get insuranceInfo(): InsuranceOption {
    return this.insurance;
  }
  public set insuranceInfo(insurance: InsuranceOption) {
    this.insurance = insurance;
  }

  public get householdAffectedInfo(): string {
    return this.householdAffected;
  }
  public set householdAffectedInfo(householdAffected: string) {
    this.householdAffected = householdAffected;
  }

  public get emergencySupportServiceS(): string {
    return this.emergencySupportServices;
  }
  public set emergencySupportServiceS(emergencySupportServices: string) {
    this.emergencySupportServices = emergencySupportServices;
  }

  public get referredServiceS(): boolean {
    return this.referredServices;
  }
  public set referredServiceS(referredServices: boolean) {
    this.referredServices = referredServices;
  }

  public get referredServiceDetailS(): string[] {
    return this.referredServiceDetails;
  }
  public set referredServiceDetailS(referredServiceDetails: string[]) {
    this.referredServiceDetails = referredServiceDetails;
  }

  public get externalServiceS(): string {
    return this.externalServices;
  }
  public set externalServiceS(externalServices: string) {
    this.externalServices = externalServices;
  }

  public get haveHouseHoldMembers(): boolean {
    return this.haveHouseholdMembers;
  }
  public set haveHouseHoldMembers(haveHouseholdMembers: boolean) {
    this.haveHouseholdMembers = haveHouseholdMembers;
  }

  public get houseHoldMembers(): PersonDetails[] {
    return this.householdMembers;
  }
  public set houseHoldMembers(householdMembers: PersonDetails[]) {
    this.householdMembers = householdMembers;
  }

  public get houseHoldMember(): PersonDetails {
    return this.householdMember;
  }
  public set houseHoldMember(householdMember: PersonDetails) {
    this.householdMember = householdMember;
  }

  public get sameLastNameChecK(): boolean {
    return this.sameLastNameCheck;
  }
  public set sameLastNameChecK(sameLastNameCheck: boolean) {
    this.sameLastNameCheck = sameLastNameCheck;
  }

  public get haveSpecialDieT(): boolean {
    return this.haveSpecialDiet;
  }
  public set haveSpecialDieT(haveSpecialDiet: boolean) {
    this.haveSpecialDiet = haveSpecialDiet;
  }

  public get specialDietDetailS(): string {
    return this.specialDietDetails;
  }
  public set specialDietDetailS(specialDietDetails: string) {
    this.specialDietDetails = specialDietDetails;
  }

  public get haveMedicatioN(): boolean {
    return this.haveMedication;
  }
  public set haveMedicatioN(haveMedication: boolean) {
    this.haveMedication = haveMedication;
  }

  public get medicationSupplY(): boolean {
    return this.medicationSupply;
  }
  public set medicationSupplY(medicationSupply: boolean) {
    this.medicationSupply = medicationSupply;
  }

  public get nextTabUpdate(): Subject<void> {
    return this.setNextTabUpdate;
  }
  public set nextTabUpdate(setNextTabUpdate: Subject<void>) {
    this.setNextTabUpdate = setNextTabUpdate;
  }

  public get tabs(): Array<TabModel> {
    return this.essTabs;
  }
  public set tabs(tabs: Array<TabModel>) {
    this.essTabs = tabs;
  }

  public setTabStatus(name: string, status: string): void {
    this.tabs.map((tab) => {
      if (tab.name === name) {
        tab.status = status;
      }
      return tab;
    });
  }

  public createNeedsAssessmentDTO(): NeedsAssessment {
    console.log({
      paperESSFile: this.paperESSFile,
      facilityName: this.facilityName,
      insurance: this.insurance,
      householdAffected: this.householdAffected,
      emergencySupportServices: this.emergencySupportServices,
      referredServices: this.referredServices,
      referredServiceDetails: this.referredServiceDetails,
      externalServices: this.externalServices,
      evacAddress: this.setAddressObject(this.evacAddress)
    });

    return {
      paperESSFile: this.paperESSFile,
      facilityName: this.facilityName,
      insurance: this.insurance,
      householdAffected: this.householdAffected,
      emergencySupportServices: this.emergencySupportServices,
      referredServices: this.referredServices,
      referredServiceDetails: this.referredServiceDetails,
      externalServices: this.externalServices
    };
  }

  /**
   * Determines if the tab navigation is allowed or not
   *
   * @param tabRoute clicked route
   * @param $event mouse click event
   * @returns true/false
   */
  isAllowed(tabRoute: string, $event: MouseEvent): boolean {
    if (tabRoute === 'review') {
      const allow = this.checkTabsStatus();
      if (allow) {
        $event.stopPropagation();
        $event.preventDefault();
        this.openModal(globalConst.wizardESSFileMessage);
      }
      return allow;
    }
  }

  /**
   * Checks the status of the tabs
   *
   * @returns true/false
   */
  checkTabsStatus(): boolean {
    return this.tabs.some(
      (tab) =>
        (tab.status === 'not-started' || tab.status === 'incomplete') &&
        tab.name !== 'review'
    );
  }

  /**
   * Open information modal window
   *
   * @param text text to display
   */
  openModal(text: string): void {
    this.dialog.open(DialogComponent, {
      data: {
        component: InformationDialogComponent,
        text
      },
      height: '230px',
      width: '530px'
    });
  }

  public setAddressObject(addressObject: AddressModel): Address {
    const address: Address = {
      addressLine1: addressObject.addressLine1,
      addressLine2: addressObject.addressLine2,
      countryCode: addressObject.country.code,
      communityCode:
        addressObject.community.code === undefined
          ? null
          : addressObject.community.code,
      postalCode: addressObject.postalCode,
      stateProvinceCode:
        addressObject.stateProvince === null
          ? null
          : addressObject.stateProvince.code
    };

    return address;
  }

  /**
   * Checks if the form is partially completed or not
   *
   * @param form form group
   * @returns true/false
   */
  checkForPartialUpdates(form: FormGroup): boolean {
    const fields = [];
    Object.keys(form.controls).forEach((field) => {
      const control = form.controls[field] as
        | FormControl
        | FormGroup
        | FormArray;
      if (control instanceof FormControl) {
        fields.push(control.value);
      } else if (control instanceof FormGroup || control instanceof FormArray) {
        for (const key in control.controls) {
          if (control.controls.hasOwnProperty(key)) {
            fields.push(control.controls[key].value);
          }
        }
      }
    });

    const result = fields.filter((field) => !!field);
    return result.length !== 0;
  }
}