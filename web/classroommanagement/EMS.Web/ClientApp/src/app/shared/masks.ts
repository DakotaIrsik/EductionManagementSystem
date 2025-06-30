import createNumberMask from 'text-mask-addons/dist/createNumberMask';

export class Masks {

  wholeNumbers(limit: number) {
    return createNumberMask({ integerLimit: limit, prefix: '' });
  }

  dollarsWithCents(limit: number) {
    return createNumberMask({ integerLimit: limit, allowDecimal: true, prefix: '' });
  }
}
