export interface IProduct {
    id: string
    name: string
    description: string
    imageFile: string
    summary: string
    price: number
    brand: IBrand
    type: IType
}

export interface IBrand {
    name: string
    id: string
}

export interface IType {
    name: string
    id: string
}
