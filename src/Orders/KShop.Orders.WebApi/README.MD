

�����
	��������� - �������� � �������� ������
		None
		Penging
		Completed
		Cancelled
		
		Refunding
		RefuncSuccessed
		RefundCancelled

	������������� ��������� - �������� � OrderSagaInstance ��� OrderRefundSagaInstance (��� ��������)
		
		


���� � ������

{...} - Bus message (Event, Request)
<...> - Saga activity
[...] - Courier Activity


	{OrderSubmit_SagaRequest}
		<OrderCreate_SagaActivity>
			[OrderCreate_CourierActivity]
			[ReserveCreate_CourierActivity]
			[InvoiceCreate_CourierActivity]
	{InvoicePayed_SagaEvent}
		<CreateInvoice_SagaActivity>
			[InvoicePay_CourierActivity]
			[ReserveFetch_CourierActivity]
			[ShipmentCreate_CourierActivity]	
	{ShipmentCompleted_SagaEvent}
		<CreateShipment_SagaActivity>
			[ShipmentComplete_CourierActivity]