#include <efi.h>
#include <efilib.h>


EFI_STATUS efi_main(EFI_HANDLE image_handle, EFI_SYSTEM_TABLE *systab)
{
    InitializeLib(image_handle, systab);

    EFI_BOOT_SERVICES* bs = systab->BootServices;
    EFI_STATUS result;
    UINTN map_size = 0;
    EFI_MEMORY_DESCRIPTOR* map;
    UINTN map_key;
    UINTN descr_size;
    UINT32 descr_version; 
    
    result = systab->BootServices->GetMemoryMap(&map_size, NULL, NULL, NULL, NULL);
    if(result != EFI_BUFFER_TOO_SMALL)
    {
        Print(L"Getting size of memory map failed.\n");
        return result;
    }
    

    result = systab->BootServices->AllocatePool(EfiLoaderData, map_size, &map);
    
    if(result != EFI_SUCCESS)
    {
        Print(L"Memory allocation failed.\n");
        bs->FreePool(map);
        return result;
    }

    result = bs->GetMemoryMap(&map_size, map, &map_key, &descr_size, &descr_version);

    if(result != EFI_SUCCESS)
    {
        Print(L"Can't get memory map.\n");
        return result;
    }

    UINT64 memory_total_size = 0;
    int descr_count = map_size/descr_size;
    int i;

    for(i = 0; i < descr_count; ++i)
    {
        switch(map[i].Type)
        {
            case EfiLoaderData:
            case EfiBootServicesData:
            case EfiRuntimeServicesData:
            case EfiConventionalMemory:
                memory_total_size += map[i].NumberOfPages;
        }
    }
    bs->FreePool(map);
   
    memory_total_size *= 4096;
    Print(L"Available memory %d bytes.\n", memory_total_size);
    return EFI_SUCCESS;
}
