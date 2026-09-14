const formatter = new Intl.NumberFormat('ru-RU', { maximumFractionDigits: 0 })

// Currency isn't modeled on the backend at all (Order/product Price is a bare decimal, no ISO
// code anywhere) - showing "сомони" is a guess matching the project's Tajikistan-flavored seed
// data (see subcategory/user names), not something read off an API field.
export function Money({ amount }: { amount: number }) {
  return <span>{formatter.format(amount)} смн</span>
}
